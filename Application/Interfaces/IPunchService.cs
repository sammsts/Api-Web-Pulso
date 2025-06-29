using ApiWebPulso.Contracts.Dtos;
using ApiWebPulso.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPunchService
    {
        Task<List<Punch>> GetHistoryAsync(Guid userId);
        Task<Punch> PunchAsync(Guid userId, PunchDto dto);
        Task<List<Punch>> GetPunchesOfDay(Guid userId);
        Task<Punch> UpdatePunchAsync(Guid userId, PunchDto dto);
        Task<List<Punch>> GetFilteredPunchesAsync(Guid userId, ReportFilterDto filters);
        Task DeletePunchAsync(Guid punchId);
    }
}
