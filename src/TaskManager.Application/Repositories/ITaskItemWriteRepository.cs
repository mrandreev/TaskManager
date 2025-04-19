using TaskManager.Domain.Entities;

namespace TaskManager.Application.Repositories;

public interface ITaskItemWriteRepository
{
    Task<TaskItem?> GetAsync(int id);
    Task<int> AddAsync(TaskItem item);
    Task UpdateAsync(TaskItem item);
}
