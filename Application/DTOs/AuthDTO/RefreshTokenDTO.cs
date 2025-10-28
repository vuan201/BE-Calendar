namespace Application.DTOs.AuthDTO;

public class RefreshTokenDTO
{
    public string TokenValue { get; set; } = string.Empty;
    public long ExpiresTime { get; set; }
}
