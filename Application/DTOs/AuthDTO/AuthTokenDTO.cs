using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AuthDTO;

public class AuthTokenDTO
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
    [Required]
    public long ExpiresTime { get; set; }
}
