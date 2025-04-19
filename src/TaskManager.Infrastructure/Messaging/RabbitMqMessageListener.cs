using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManager.Application.Messaging.Interfaces;

namespace TaskManager.Infrastructure.Messaging;

public class RabbitMqMessageListener<TMessage> : BackgroundService
{
    private readonly ILogger<RabbitMqMessageListener<TMessage>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqOptions _options;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;
    public RabbitMqMessageListener(
        ILogger<RabbitMqMessageListener<TMessage>> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqOptions> options)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _queueName = typeof(TMessage).Name;

        _factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());
                    var message = JsonSerializer.Deserialize<TMessage>(json);

                    if (message is null)
                    {
                        _logger.LogWarning($"Received null or invalid message of type {typeof(TMessage).Name}");
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetService<IMessageHandler<TMessage>>();

                    await handler.HandleAsync(message, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error handling '{typeof(TMessage).Name}': {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: cancellationToken
            );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to start RabbitMQ listener for message '{typeof(TMessage).Name}': {ex.Message}");
        }
    }
}
