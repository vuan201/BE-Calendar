using Application.DTOs.AuthDTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
