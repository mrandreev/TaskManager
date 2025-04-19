using TaskManager.Core.Domain;

namespace TaskManager.Domain.Events;

public sealed class TaskItemCompletedEvent(int taskId, string name) : IDomainEvent
{
    public int TaskId { get; } = taskId;
    public string Name { get; } = name;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
