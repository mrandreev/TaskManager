using TaskManager.Core.Models;

namespace TaskManager.Application.TaskItems.Models;

public class TaskItemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public EnumModel Status { get; set; }
}
