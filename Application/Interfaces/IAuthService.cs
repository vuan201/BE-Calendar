using Application.DTOs.AuthDTO;
using Application.Models;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<ViewModel<AuthTokenDTO>> Register(AuthRegisterDTO register);
    Task<ViewModel<AuthTokenDTO>> Login(AuthLoginDTO login);
    Task<ViewModel> Logout(AuthTokenDTO authTokenDTO);
    Task<ViewModel<AuthTokenDTO>> RefreshToken(AuthTokenDTO authTokenDTO);
}
