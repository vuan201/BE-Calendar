using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTO;

public class AuthRegisterDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters, including uppercase, lowercase, numbers and special characters.")]
    public string Password { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    [StringLength(32)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(32)]
    public string LastName { get; set; } = string.Empty;
    [StringLength(128)]
    public string Address { get; set; } = string.Empty;
}
