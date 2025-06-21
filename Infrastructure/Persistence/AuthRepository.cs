using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IGenericService<User> _userService;

        public AuthRepository(IGenericService<User> userService)
        {
            _userService = userService;
        }

        public async Task<User> GetValidUserAsync(string username, string password)
        {
            try
            {
                var user = await _userService.GetFirstOrDefaultAsync(u => u.Username == username);
                if (user == null) return null;

                return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao validar usuário: {ex.Message}", ex);
            }
        }

    }
}
