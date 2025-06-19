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

    [HttpPost("{userId}")]
    public async Task<IActionResult> Punch(Guid userId, [FromBody] PunchDto dto)
    {
        var punch = await _punchService.PunchAsync(userId, dto.Type);
        return Ok(punch);
    }

    [HttpGet("{userId}/history")]
    public async Task<IActionResult> History(Guid userId)
    {
        var history = await _punchService.GetHistoryAsync(userId);
        return Ok(history);
    }
}