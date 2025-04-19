namespace TaskManager.Application.Messaging.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default);
}
