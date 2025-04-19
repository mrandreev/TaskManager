using System.ComponentModel;

namespace TaskManager.Domain.Enums;

public enum TaskItemStatus
{
    [Description("Not Started")]
    NotStarted,
    [Description("In Progress")]
    InProgress,
    [Description("Completed")]
    Completed
}
