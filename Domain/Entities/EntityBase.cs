using System.ComponentModel.DataAnnotations;
using Domain.Extension;

namespace Domain.Entities;

public class EntityBase
{
    [Required]
    public long CreatedAt { get; set; } = DateTime.UtcNow.ToUnixTime();
    public long? UpdatedAt { get; set; } = DateTime.UtcNow.ToUnixTime();
    [Required]
    public bool IsDeleted { get; set; } = false;
}