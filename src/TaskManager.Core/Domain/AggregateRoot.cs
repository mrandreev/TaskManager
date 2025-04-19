namespace TaskManager.Core.Domain;

public abstract class AggregateRoot<TId>(TId identity) : Entity<TId>(identity)
{
}
