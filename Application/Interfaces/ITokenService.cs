using Application.DTOs.AuthDTO;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(ApplicationUser user, IList<string> roles);
    RefreshTokenDTO CreateRefreshToken();
}
