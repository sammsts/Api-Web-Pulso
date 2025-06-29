using ApiWebPulso.Contracts.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await _userService.RegisterAsync(dto);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetById()
    {
        var id = User.FindFirst("UserId")?.Value;
        var user = await _userService.GetByIdAsync(Guid.Parse(id));
        return Ok(user);
    }
}