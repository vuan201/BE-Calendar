using Application.DTOs.AuthDTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet("GetUserInfo", Name = "GetUserInfo")]
    public async Task<IActionResult> GetUserInfo()
    {
        var userName = _userService.GetUserName();
        if (string.IsNullOrEmpty(userName)) return BadRequest("User not found");

        var result = await _userService.GetUserInfomation(userName);

        if (!result.Status) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("Login", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDTO login)
    {
        var result = await _authService.Login(login);

        if (!result.Status) return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("Register", Name = "Register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterDTO register)
    {
        var result = await _authService.Register(register);

        if (!result.Status) return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("Logout", Name = "Logout")]
    public async Task<IActionResult> Logout([FromHeader] AuthTokenDTO token)
    {
        var result = await _authService.Logout(token);

        if (!result.Status) return BadRequest(result);

        return Ok();
    }
    [HttpGet("GetAccessToken", Name = "GetAccessToken")]
    public async Task<IActionResult> GetAccessToken([FromQuery] string refreshToken)
    {
        var result = await _authService.GetAccessToken(refreshToken);

        if (!result.Status) return BadRequest(result);
        
        return Ok(result);
    }
}
