using ApiWebPulso.Contracts.Dtos;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GeneratePunchReportAsync(Guid userId, ReportFilterDto filters);
    }

}
