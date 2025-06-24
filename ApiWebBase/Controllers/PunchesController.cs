using ApiWebPulso.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PunchesController : ControllerBase
{
    private readonly IPunchService _punchService;

    public PunchesController(IPunchService punchService)
    {
        _punchService = punchService;
    }

    [HttpPost]
    public async Task<IActionResult> Punch([FromBody] PunchDto dto)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var punch = await _punchService.PunchAsync(Guid.Parse(userId), dto.Type);
        return Ok(punch);
    }

    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var history = await _punchService.GetHistoryAsync(Guid.Parse(userId));
        return Ok(history);
    }
}