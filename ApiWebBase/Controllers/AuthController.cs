using ApiWebPulso.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        if (login.Username != "admin" || login.Password != "senha123")
            return Unauthorized();

        var accessToken = _authService.GenerateTokenAsync(login.Username, login.Password);
        var refreshToken = Guid.NewGuid().ToString();

        return Ok(new { accessToken, refreshToken });
    }
}
