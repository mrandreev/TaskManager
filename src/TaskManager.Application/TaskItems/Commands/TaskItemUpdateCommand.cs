using FluentValidation;
using MediatR;
using TaskManager.Application.Repositories;
using TaskManager.Core.Validation;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.TaskItems.Commands;

public record TaskItemUpdateCommand(int Id, string Name, string? Description) : IRequest<Unit>;

public class TaskItemUpdateCommandValidator : AbstractValidator<TaskItemUpdateCommand>
{
    public TaskItemUpdateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}

public class TaskItemUpdateCommandHandler(
    ITaskItemWriteRepository taskItemWriteRepository) : IRequestHandler<TaskItemUpdateCommand, Unit>
{
    public async Task<Unit> Handle(TaskItemUpdateCommand request, CancellationToken cancellationToken)
    {
        var task = await taskItemWriteRepository.GetAsync(request.Id);

        Contract.EntityNotNull(task, nameof(TaskItem), request.Id);

        task.Update(request.Name, request.Description);

        await taskItemWriteRepository.UpdateAsync(task);

        return Unit.Value;
    }
}