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

        public async Task<string> GenerateTokenAsync(string username, string password)
        {
            var isValid = await _authRepository.ValidateUserCredentialsAsync(username, password);

            if (!isValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            var role = await _authRepository.GetUserRoleAsync(username);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role ?? "User")
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
