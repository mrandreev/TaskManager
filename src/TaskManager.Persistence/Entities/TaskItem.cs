using System.ComponentModel.DataAnnotations;
using TaskManager.Domain.Enums;
using TaskManager.Persistence.Common;

namespace TaskManager.Persistence.Entities;

public class TaskItem : MutableDbEntity<int>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; }
}
