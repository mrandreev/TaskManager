namespace TaskManager.Application.Messaging.Interfaces;

public interface IMessageHandler<TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}