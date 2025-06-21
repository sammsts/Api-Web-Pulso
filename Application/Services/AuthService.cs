using ApiWebPulso.Contracts.Dtos;
using Application.Interfaces;
using Domain.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtOptions _options;

        public AuthService(IAuthRepository authRepository, IOptions<JwtOptions> options)
        {
            _authRepository = authRepository;
            _options = options.Value;
        }

        public async Task<AuthResultDto> GenerateTokenAsync(string username, string password)
        {
            var user = await _authRepository.GetValidUserAsync(username, password);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", username),
                new Claim("Fullname", user.FullName),
                new Claim("Email", user.Email ?? ""),
                new Claim("Role", user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiryMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResultDto
            {
                AccessToken = accessToken,
                RefreshToken = Guid.NewGuid().ToString(),
                User = new UserDto
                {
                    UserId = user.Id.ToString(),
                    Username = user.Username,
                    Fullname = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                }
            };
        }
    }
}
