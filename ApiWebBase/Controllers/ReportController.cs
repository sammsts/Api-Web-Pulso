using ApiWebPulso.Contracts.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost]
    public async Task<IActionResult> GenerateReport([FromBody] ReportFilterDto filters)
    {
        try
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var reportBytes = await _reportService.GeneratePunchReportAsync(Guid.Parse(userId), filters);

            return File(
                fileContents: reportBytes,
                contentType: "application/pdf",
                fileDownloadName: "relatorio.pdf"
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
