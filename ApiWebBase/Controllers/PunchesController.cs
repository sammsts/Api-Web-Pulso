using ApiWebPulso.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
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

        var punch = await _punchService.PunchAsync(Guid.Parse(userId), dto);
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

    [HttpGet("day")]
    public async Task<IActionResult> GetPunchesOfDay()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var punches = await _punchService.GetPunchesOfDay(Guid.Parse(userId));
        return Ok(punches);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdatePunch([FromBody] PunchDto dto)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var punch = await _punchService.UpdatePunchAsync(Guid.Parse(userId), dto);
        return Ok(punch);
    }

    [HttpDelete("{punchId}")]
    public async Task<IActionResult> DeletePunch(Guid punchId)
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        await _punchService.DeletePunchAsync(punchId);
        return Ok("Deletado com sucesso");
    }
}