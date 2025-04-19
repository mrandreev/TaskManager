using MediatR;
using TaskManager.Core.Domain;

namespace TaskManager.Application.DomainEvents;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            await mediator.Publish(CreateNotification(domainEvent), cancellationToken);
        }
    }

    private static INotification CreateNotification(IDomainEvent domainEvent)
    {
        var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
        return (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;
    }
}
