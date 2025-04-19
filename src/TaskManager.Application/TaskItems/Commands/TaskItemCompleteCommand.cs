using MediatR;
using TaskManager.Application.DomainEvents;
using TaskManager.Application.Repositories;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.TaskItems.Commands;

public record TaskItemCompleteCommand(int Id) : TaskItemStatusBaseCommand(Id);
public class TaskItemCompleteCommandHandler(
    ITaskItemWriteRepository taskItemWriteRepository,
    IDomainEventDispatcher dispatcher) : BaseTaskItemStatusCommandHandler<TaskItemCompleteCommand>(taskItemWriteRepository, dispatcher)
{
    protected override Task<Unit> Handle(TaskItem taskItem, CancellationToken cancellationToken)
    {
        taskItem.Complete();

        return Task.FromResult(Unit.Value);
    }
}