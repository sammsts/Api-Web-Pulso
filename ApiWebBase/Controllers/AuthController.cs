using ApiWebPulso.Dtos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        try
        {
            var result = await _authService.GenerateTokenAsync(login.Username, login.Password);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }
    }
}
