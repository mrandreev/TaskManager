using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TaskManager.Application.Messaging.Interfaces;

namespace TaskManager.Infrastructure.Messaging;

public class RabbitMqMessageBus : IMessageBus
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMqMessageBus> _logger;

    public RabbitMqMessageBus(IOptions<RabbitMqOptions> options, ILogger<RabbitMqMessageBus> logger)
    {
        var config = options.Value;
        _factory = new ConnectionFactory
        {
            HostName = config.HostName,
            Port = config.Port,
            UserName = config.UserName,
            Password = config.Password,
            VirtualHost = config.VirtualHost
        };
        _logger = logger;
    }

    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var queueName = typeof(TMessage).Name;

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: typeof(TMessage).Name,
                body: body,
                cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to publish message of type '{typeof(TMessage).Name}': {ex.Message}");
        }
    }
}
