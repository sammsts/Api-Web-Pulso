using ApiWebPulso.Contracts.Dtos;
using Application.Utils;
using Application.Interfaces;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IPunchService _punchService;

        public ReportService(IPunchService punchService)
        {
            _punchService = punchService;
        }

        public async Task<byte[]> GeneratePunchReportAsync(Guid userId, ReportFilterDto filters)
        {
            var punches = await _punchService.GetFilteredPunchesAsync(userId, filters);

            var generator = new PdfReportGenerator();
            return generator.Generate(punches);
        }
    }

}
