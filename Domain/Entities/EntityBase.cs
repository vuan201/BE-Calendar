using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class EntityBase
{
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    [Required]
    public bool IsDeleted { get; set; } = false;
    public virtual IEnumerable<Event>? Events{ get; set; } 
}