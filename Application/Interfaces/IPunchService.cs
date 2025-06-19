using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPunchService
    {
        Task<List<Punch>> GetHistoryAsync(Guid userId);
        Task<Punch> PunchAsync(Guid userId, string type);
    }
}
