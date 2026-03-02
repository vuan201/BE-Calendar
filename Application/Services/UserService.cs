using System.Security.Claims;
using Application.DTOs.UserDTO;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public string? GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<ViewModel<UserInfomationDTO>> GetUserInfomation(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null)
        {
            return new ViewModel<UserInfomationDTO>(true, "Get user information success", _mapper.Map<UserInfomationDTO>(user));
        }
        return new ViewModel<UserInfomationDTO>(false, "User not found");
    }

    public string? GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
    }

    public List<string> GetUserRoles()
    {
        return _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList() ?? new List<string>();
    }
}
