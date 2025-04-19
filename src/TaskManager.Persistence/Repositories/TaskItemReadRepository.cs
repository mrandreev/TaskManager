using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Repositories;
using TaskManager.Application.TaskItems.Models;
using TaskManager.Core.Extensions;
using TaskManager.Persistence.DbContexts;
using DAL = TaskManager.Persistence.Entities;

namespace TaskManager.Persistence.Repositories;

public class TaskItemReadRepository(AppDbContext dbContext) : ITaskItemReadRepository
{
    public async Task<ICollection<TaskItemModel>> GetAllAsync()
    {
        var tasks = await dbContext.Tasks.AsNoTracking().ToListAsync();

        return tasks.Select(Map).ToList();
    }

    private static TaskItemModel Map(DAL.TaskItem item)
    {
        return new TaskItemModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status.ToEnumModel()
        };
    }
}
