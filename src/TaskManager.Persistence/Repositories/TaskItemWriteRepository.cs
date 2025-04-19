using TaskManager.Application.Repositories;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Persistence.DbContexts;
using DAL = TaskManager.Persistence.Entities;

namespace TaskManager.Persistence.Repositories;

public class TaskItemWriteRepository(AppDbContext dbContext) : ITaskItemWriteRepository
{
    public async Task<TaskItem?> GetAsync(int id)
    {
        var taskDb = await dbContext.Tasks.FindAsync(id);
        return taskDb != null ? MapTask(taskDb) : null;
    }

    public async Task<int> AddAsync(TaskItem task)
    {
        var taskDb = MapTask(task);
        dbContext.Tasks.Add(taskDb);

        await dbContext.SaveChangesAsync();

        return taskDb.Id;
    }

    public async Task UpdateAsync(TaskItem task)
    {
        var taskDb = await dbContext.Tasks.FindAsync(task.Id);

        taskDb = MapTask(task, taskDb);

        dbContext.Tasks.Update(taskDb);

        await dbContext.SaveChangesAsync();
    }

    private static DAL.TaskItem MapTask(TaskItem task, DAL.TaskItem? taskDb = null)
    {
        taskDb ??= new DAL.TaskItem();
        taskDb.Id = task.Id;
        taskDb.Name = task.Name;
        taskDb.Description = task.Description;
        taskDb.Status = task.Status;

        return taskDb;
    }

    private static TaskItem MapTask(DAL.TaskItem task)
    {
        return new TaskItem(
            id: task.Id,
            name: task.Name,
            description: task.Description,
            status: task.Status
        );
    }
}
