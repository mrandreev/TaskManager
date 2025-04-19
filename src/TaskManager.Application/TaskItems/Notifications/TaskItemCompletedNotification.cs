using MediatR;
using TaskManager.Application.DomainEvents;
using TaskManager.Application.Messaging.Interfaces;
using TaskManager.Application.Messaging.Messages;
using TaskManager.Domain.Events;

namespace TaskManager.Application.TaskItems.Notifications;

public class TaskItemCompletedNotificationHandler(IMessageBus messageBus) : INotificationHandler<DomainEventNotification<TaskItemCompletedEvent>>
{
    public Task Handle(DomainEventNotification<TaskItemCompletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var message = new TaskItemCompletedMessage(domainEvent.TaskId, domainEvent.Name, domainEvent.OccurredOn);

        return messageBus.PublishAsync(message, cancellationToken);
    }
}
