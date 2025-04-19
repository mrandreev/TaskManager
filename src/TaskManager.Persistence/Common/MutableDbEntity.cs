using System.ComponentModel.DataAnnotations;

namespace TaskManager.Persistence.Common;

public class MutableDbEntity<TId> : DbEntity<TId>, IMutable
{
    [Required]
    public DateTime CreatedDate { get; set; }
    public DateTime? ChangedDate { get; set; }
}
