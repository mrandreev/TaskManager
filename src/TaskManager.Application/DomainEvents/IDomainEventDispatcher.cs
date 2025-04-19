using TaskManager.Core.Domain;

namespace TaskManager.Application.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}
