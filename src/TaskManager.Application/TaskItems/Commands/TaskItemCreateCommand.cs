using FluentValidation;
using MediatR;
using TaskManager.Application.Repositories;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.TaskItems.Commands;

public record TaskItemCreateCommand(string Name, string? Description) : IRequest<int>;

public class TaskItemCreateCommandValidator : AbstractValidator<TaskItemCreateCommand>
{
    public TaskItemCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}

public class TaskItemCreateCommandHandler(
    ITaskItemWriteRepository taskItemWriteRepository) : IRequestHandler<TaskItemCreateCommand, int>
{
    public Task<int> Handle(TaskItemCreateCommand request, CancellationToken cancellationToken)
    {
        var task = TaskItem.Create(request.Name, request.Description);

        return taskItemWriteRepository.AddAsync(task);
    }
}