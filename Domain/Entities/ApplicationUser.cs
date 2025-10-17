
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [StringLength(500)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(500)]
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    // public int? AvatarId { get; set; }

    // [ForeignKey(nameof(AvatarId))]
    // public virtual Files? Avatar { get; set; }
}