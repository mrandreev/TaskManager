namespace TaskManager.Core.Domain;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
