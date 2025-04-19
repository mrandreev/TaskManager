using System.ComponentModel.DataAnnotations;

namespace TaskManager.Persistence.Common;

public class DbEntity<TId>
{
    [Required]
    public virtual TId Id { get; set; }
}
