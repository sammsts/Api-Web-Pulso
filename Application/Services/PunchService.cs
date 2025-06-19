using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PunchService : IPunchService
    {
        private readonly IGenericService<Punch> _service;

        public PunchService(IGenericService<Punch> service)
        {
            _service = service;
        }

        public async Task<Punch> PunchAsync(Guid userId, string type)
        {
            var punch = new Punch
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Type = type
            };
            await _service.CreateAsync(punch);
            return punch;
        }

        public async Task<List<Punch>> GetHistoryAsync(Guid userId)
        {
            var all = await _service.GetAllAsync();
            return all.Where(p => p.UserId == userId).OrderByDescending(p => p.Timestamp).ToList();
        }
    }
}
