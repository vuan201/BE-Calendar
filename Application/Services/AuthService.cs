using Application.DTOs.AuthDTO;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.constants;
using Domain.Entities;
using Domain.Extension;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepository;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, ITokenRepository tokenRepository, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _tokenRepository = tokenRepository;
        _mapper = mapper;
    }
    public async Task<ViewModel<AuthTokenDTO>> Login(AuthLoginDTO login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
        {
            // *Lấy Roles và tạo Access Token
            var userRole = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.CreateAccessToken(user, userRole);

            // *Tạo Refresh Token và lưu vào DB (Thời gian dài)
            var refreshToken = _tokenService.CreateRefreshToken();

            // *Lưu Refresh Token vào DB
            var token = new Token
            {
                UserId = user.Id,
                TokenValue = refreshToken.TokenValue,
                ExpiresTime = refreshToken.ExpiresTime,
                IsRevoked = false
            };

            await _tokenRepository.CreateAsync(token);
            await _tokenRepository.SaveChangesAsync();

            var result = new AuthTokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenValue,
                ExpiresTime = refreshToken.ExpiresTime
            };
            return new ViewModel<AuthTokenDTO>(true, "Login success", result);
        }
        return new ViewModel<AuthTokenDTO>(false, "Invalid credentials");
    }

    public async Task<ViewModel> Logout(AuthTokenDTO authTokenDTO)
    {
        // * Kiểm tra Refresh Token có hợp lệ hay không
        var token = await _tokenRepository.GetTokenByValueAsync(authTokenDTO.RefreshToken);

        if (token == null) return new ViewModel(false, "Invalid refresh token");

        // * Thu hồi token đã sử dụng 
        token.IsRevoked = true;
        await _tokenRepository.SaveChangesAsync();

        return new ViewModel(true, "Logout success");
    }

    public async Task<ViewModel<AuthTokenDTO>> GetAccessToken(string refreshToken)
    {
        var token = await _tokenRepository.GetTokenByValueAsync(refreshToken);

        if (token == null) return new ViewModel<AuthTokenDTO>(false, "Invalid refresh token");
        if (token.IsRevoked)
        {
            return new ViewModel<AuthTokenDTO>(false, "Refresh token has been revoked");
        }
        else if (token.ExpiresTime < DateTime.UtcNow.ToUnixTime())
        {
            token.IsRevoked = true;
            await _tokenRepository.SaveChangesAsync();

            return new ViewModel<AuthTokenDTO>(false, "Refresh token has expired");
        }

        // * Lấy User từ Refresh Token
        var user = await _userManager.FindByIdAsync(token.UserId);

        if (user == null) return new ViewModel<AuthTokenDTO>(false, "Error getting user from database");

        // * Thu hồi token cũ và bắt đầu tạo một rotasion token

        // * Thu hồi Refresh Token cũ
        token.IsRevoked = true;
        await _tokenRepository.SaveChangesAsync();

        // *Lấy Roles và tạo Access Token
        var userRole = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.CreateAccessToken(user, userRole);

        // *Tạo Refresh Token và lưu vào DB (Thời gian dài)
        var newRefreshToken = _tokenService.CreateRefreshToken();

        // *Lưu Refresh Token vào DB
        var newToken = new Token
        {
            UserId = user.Id,
            TokenValue = newRefreshToken.TokenValue,
            ExpiresTime = newRefreshToken.ExpiresTime,
            IsRevoked = false
        };

        await _tokenRepository.CreateAsync(newToken);
        await _tokenRepository.SaveChangesAsync();

        var result = new AuthTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.TokenValue,
            ExpiresTime = newRefreshToken.ExpiresTime
        };
        return new ViewModel<AuthTokenDTO>(true, "Get access token success", result);
    }

    public async Task<ViewModel<AuthTokenDTO>> Register(AuthRegisterDTO register)
    {
        var user = _mapper.Map<ApplicationUser>(register);

        // * Tạo người dùng mới
        var result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            return new ViewModel<AuthTokenDTO>(false, "Register failed")
            {
                Errors = result.Errors.ToList()
            };
        }

        // * Thêm role cho người dùng
        await _userManager.AddToRoleAsync(user, Roles.User);

        // * Tạo access token
        var accessToken = _tokenService.CreateAccessToken(user, new List<string> { Roles.User });

        // *Tạo Refresh Token và lưu vào DB (Thời gian dài)
        var refreshToken = _tokenService.CreateRefreshToken();

        // *Lưu Refresh Token vào DB
        var token = new Token
        {
            UserId = user.Id,
            TokenValue = refreshToken.TokenValue,
            ExpiresTime = refreshToken.ExpiresTime,
            IsRevoked = false
        };
        await _tokenRepository.CreateAsync(token);
        await _tokenRepository.SaveChangesAsync();

        var authTokenDTO = new AuthTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.TokenValue,
            ExpiresTime = refreshToken.ExpiresTime
        };

        return new ViewModel<AuthTokenDTO>(true, "Register success", authTokenDTO);
    }
}
