using TaskManager.Domain.Enums;
using TaskManager.Core.Validation;
using TaskManager.Core.Domain;
using TaskManager.Domain.Events;

namespace TaskManager.Domain.Entities;

public class TaskItem(
    int id,
    string name,
    string? description,
    TaskItemStatus status) : AggregateRoot<int>(id)
{
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public TaskItemStatus Status { get; private set; } = status;

    public static TaskItem Create(string name, string? description)
    {
        Contract.NotNullOrEmpty(name, nameof(name));
        Contract.MaxLength(name, 100, nameof(description));

        return new TaskItem(default, name, description, TaskItemStatus.NotStarted);
    }

    public void Update(string name, string? description)
    {
        Contract.NotNullOrEmpty(name, nameof(name));

        Name = name;
        Description = description;
    }

    public void Start()
    {
        Contract.Requires(Status == TaskItemStatus.NotStarted, "Task must be Not Started to start");

        Status = TaskItemStatus.InProgress;
    }

    public void Complete()
    {
        Contract.Requires(Status == TaskItemStatus.InProgress, "Task must be In Progress to complete");

        Status = TaskItemStatus.Completed;

        EmitEvent(new TaskItemCompletedEvent(Id, Name));
    }
}
