using TaskManager.Core.Validation;

namespace TaskManager.Core.Domain;

public abstract class Entity<TId>
{
    private readonly List<IDomainEvent> domainEvents = [];

    public TId Id { get; }

    public IReadOnlyCollection<IDomainEvent> Events => domainEvents.AsReadOnly();

    protected Entity(TId id)
    {
        Contract.Requires(id != null, $"{nameof(id)} != null");

        Id = id;
    }

    protected void EmitEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
