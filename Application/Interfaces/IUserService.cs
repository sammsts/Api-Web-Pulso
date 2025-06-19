using Domain.Entities;
using ApiWebPulso.Contracts.Dtos;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<User> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
    }
}
