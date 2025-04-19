namespace TaskManager.Persistence.Common;

public interface IMutable
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ChangedDate { get; set; }
}
