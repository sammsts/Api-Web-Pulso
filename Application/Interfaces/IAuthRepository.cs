using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetValidUserAsync(string username, string password);
    }
}
