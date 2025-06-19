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

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            try
            {
                var user = await _userService.GetFirstOrDefaultAsync(u => u.Username == username);
                if (user == null) return false;

                return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao validar usuário: {ex.Message}", ex);
            }
        }

        public async Task<string?> GetUserRoleAsync(string username)
        {
            var user = await _userService.GetFirstOrDefaultAsync(u => u.Username == username);
            return user?.Role ?? "User";
        }
    }
}
