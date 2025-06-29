using ApiWebPulso.Contracts.Dtos;
using ApiWebPulso.Dtos;
using Application.Interfaces;
using Domain.Entities;
using System.Runtime.InteropServices;

namespace Application.Services
{
    public class PunchService : IPunchService
    {
        private readonly IGenericService<Punch> _service;

        public PunchService(IGenericService<Punch> service)
        {
            _service = service;
        }

        public async Task<Punch> PunchAsync(Guid userId, PunchDto dto)
        {
            var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "E. South America Standard Time"
                : "America/Sao_Paulo";

            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var localToday = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).Date;

            var startLocal = localToday;
            var endLocal = localToday.AddDays(1);

            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tz);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, tz);

            var punchesToday = await _service.GetWhereAsync(x =>
                x.UserId == userId &&
                x.Timestamp >= startUtc &&
                x.Timestamp < endUtc
            );

            var lastPunch = punchesToday.OrderByDescending(p => p.Timestamp).FirstOrDefault();

            var newType = (lastPunch == null || (PunchType)lastPunch.Type == PunchType.Out)
                ? PunchType.In
                : PunchType.Out;

            var punch = new Punch
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Timestamp = dto.Timestamp ?? DateTime.UtcNow,
                Type = dto.ManuallyEdited ? (int)dto.Type : (int)newType,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                ManuallyEdited = dto.ManuallyEdited
            };

            await _service.CreateAsync(punch);
            return punch;
        }

        public async Task<List<Punch>> GetHistoryAsync(Guid userId)
        {
            var all = await _service.GetAllAsync();
            return all.Where(p => p.UserId == userId).OrderByDescending(p => p.Timestamp).ToList();
        }

        public async Task<List<Punch>> GetPunchesOfDay(Guid userId)
        {
            // fuso horário de Brasília
            var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "E. South America Standard Time"
                : "America/Sao_Paulo";

            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Data atual em Brasília (sem hora)
            var localToday = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).Date;

            // Intervalo local
            var startLocal = localToday;
            var endLocal = localToday.AddDays(1);

            // Converte para UTC antes de consultar no banco
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tz);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, tz);

            return await _service.GetWhereAsync(x =>
                x.UserId == userId &&
                x.Timestamp >= startUtc &&
                x.Timestamp < endUtc
            );
        }

        public async Task<Punch> UpdatePunchAsync(Guid userId, PunchDto dto)
        {
            if (dto.Id == Guid.Empty)
            {
                throw new InvalidOperationException("Ponto não encontrado.");
            }

            var existingPunch = await _service.GetByIdAsync((Guid)dto.Id);
            if (existingPunch == null || existingPunch.UserId != userId)
            {
                throw new InvalidOperationException("Ponto não encontrado.");
            }

            existingPunch.Timestamp = DateTime.SpecifyKind(dto.Timestamp.Value, DateTimeKind.Utc);
            existingPunch.Type = (int)dto.Type;
            existingPunch.Latitude = dto.Latitude;
            existingPunch.Longitude = dto.Longitude;
            existingPunch.Address = dto.Address;
            existingPunch.ManuallyEdited = true;
            existingPunch.UpdatedAt = DateTime.UtcNow;
            existingPunch.CreatedAt = DateTime.SpecifyKind((DateTime)existingPunch.CreatedAt, DateTimeKind.Utc);

            await _service.UpdateAsync(existingPunch);
            return existingPunch;
        }

        public async Task<List<Punch>> GetFilteredPunchesAsync(Guid userId, ReportFilterDto filters)
        {
            var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "E. South America Standard Time"
                : "America/Sao_Paulo";

            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (filters.StartDate == default || filters.EndDate == default)
                throw new ArgumentException("Data Início e Data Fim são obrigatórios.");

            var startLocal = DateTime.SpecifyKind(filters.StartDate.Value.Date, DateTimeKind.Unspecified);
            var endLocal = DateTime.SpecifyKind(filters.EndDate.Value.Date.AddDays(1), DateTimeKind.Unspecified);

            var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tz);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, tz);

            var punches = await _service.GetWhereAsync(p =>
                p.UserId == userId &&
                (p.Timestamp >= startUtc) &&
                (p.Timestamp < endUtc) &&
                (!filters.Type.HasValue || p.Type == (int)filters.Type.Value)
            );

            return punches.OrderBy(p => p.Timestamp).ToList();
        }

        public async Task DeletePunchAsync(Guid punchId)
        {
            try
            {
                await _service.DeleteAsync(punchId);
            }
            catch
            {
                throw new InvalidOperationException("Erro ao excluir o ponto.");
            }
        }
    }
}
