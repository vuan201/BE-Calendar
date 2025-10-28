using Application.DTOs.UserDTO;
using Application.Models;

namespace Application.Interfaces;

public interface IUserService
{
    string? GetUserId();
    string? GetUserName();
    List<string> GetUserRoles();
    Task<ViewModel<UserInfomationDTO>> GetUserInfomation(string username);
}
