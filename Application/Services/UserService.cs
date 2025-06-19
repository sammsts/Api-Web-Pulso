using Application.Interfaces;
using Domain.Entities;
using ApiWebPulso.Contracts.Dtos;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericService<User> _service;

        public UserService(IGenericService<User> service)
        {
            _service = service;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email
            };
            await _service.CreateAsync(user);
        }

        public async Task<User> GetByIdAsync(Guid id) => await _service.GetByIdAsync(id);

        public async Task<User?> GetByUsernameAsync(string username)
            => await _service.GetFirstOrDefaultAsync(u => u.Username == username);
    }
}
