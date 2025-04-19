using TaskManager.Application.TaskItems.Models;

namespace TaskManager.Application.Repositories;

public interface ITaskItemReadRepository
{
    Task<ICollection<TaskItemModel>> GetAllAsync();
}
