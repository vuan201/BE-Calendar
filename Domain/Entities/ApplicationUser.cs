
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(32)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(32)]
    public string LastName { get; set; } = string.Empty;
    [StringLength(128)]
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public virtual IEnumerable<Token>? Tokens { get; set; } = new List<Token>();
    public virtual IEnumerable<Event>? Events { get; set; } = new List<Event>();
}