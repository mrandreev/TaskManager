using MediatR;
using TaskManager.Application.Repositories;
using TaskManager.Application.TaskItems.Models;

namespace TaskManager.Application.TaskItems.Queries;

public record TaskItemsQuery : IRequest<ICollection<TaskItemModel>>;

public class TaskItemsQueryHandler(
    ITaskItemReadRepository taskItemReadRepository) : IRequestHandler<TaskItemsQuery, ICollection<TaskItemModel>>
{
    public Task<ICollection<TaskItemModel>> Handle(TaskItemsQuery request, CancellationToken cancellationToken)
    {
        return taskItemReadRepository.GetAllAsync();
    }
}