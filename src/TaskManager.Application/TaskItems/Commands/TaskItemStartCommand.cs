using MediatR;
using TaskManager.Application.DomainEvents;
using TaskManager.Application.Repositories;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.TaskItems.Commands;

public record TaskItemStartCommand(int Id) : TaskItemStatusBaseCommand(Id);
public class TaskItemStartCommandHandler(
    ITaskItemWriteRepository taskItemWriteRepository,
    IDomainEventDispatcher dispatcher) : BaseTaskItemStatusCommandHandler<TaskItemStartCommand>(taskItemWriteRepository, dispatcher)
{
    protected override Task<Unit> Handle(TaskItem taskItem, CancellationToken cancellationToken)
    {
        taskItem.Start();

        return Task.FromResult(Unit.Value);
    }
}