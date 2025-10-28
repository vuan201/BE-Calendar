using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs.AuthDTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateAccessToken(ApplicationUser user, IList<string> roles)
    {
        // *1. Định nghĩa Claims (User ID, Username, Roles...)
        var authClaims = new List<Claim>
        {
            // * ID của User
            new Claim(ClaimTypes.NameIdentifier, user.Id),

            // * Username của User
            new Claim(ClaimTypes.Name, user.UserName ?? ""),

            // * ID duy nhất cho token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // * Thêm các vai trò của User vào trong claims
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // *2. Cấu hình Key và Thuật toán
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"] ?? ""));

        // *3. Tạo Access Token (Thời gian ngắn)
        var token = new JwtSecurityToken(
            // Nguồn phát hành token
            issuer: _config["JWT:ValidIssuer"],
            // Nguồn được chấp nhận
            audience: _config["JWT:ValidAudience"],
            // Thời hạn tồn tại của token
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JWT:AccessTokenExpireMinutes"])),
            // Các thông tin khác về người dùng
            claims: authClaims,
            // Thuật toán mã hóa và chữ ký
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenDTO CreateRefreshToken()
    {
        var expiresTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["JWT:RefreshTokenExpireDays"])).ToUnixTime();

        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return new RefreshTokenDTO { TokenValue = Convert.ToBase64String(randomNumber), ExpiresTime = expiresTime };
        }
    }
}
