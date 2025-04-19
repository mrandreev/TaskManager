using MediatR;
using TaskManager.Application.DomainEvents;
using TaskManager.Application.Repositories;
using TaskManager.Core.Validation;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.TaskItems.Commands;

public record TaskItemStatusBaseCommand(int Id) : IRequest<Unit>;
public abstract class BaseTaskItemStatusCommandHandler<TRequest>(
    ITaskItemWriteRepository taskItemWriteRepository,
    IDomainEventDispatcher dispatcher) : IRequestHandler<TRequest, Unit>
    where TRequest : TaskItemStatusBaseCommand
{
    public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var task = await taskItemWriteRepository.GetAsync(request.Id);
        Contract.EntityNotNull(task, nameof(TaskItem), request.Id);

        await Handle(task, cancellationToken);

        await taskItemWriteRepository.UpdateAsync(task);
        await dispatcher.DispatchAsync(task.Events, cancellationToken);

        return Unit.Value;
    }

    protected abstract Task<Unit> Handle(TaskItem taskItem, CancellationToken cancellationToken);
}