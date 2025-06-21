using ApiWebPulso.Contracts.Dtos;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> GenerateTokenAsync(string username, string password);
    }
}
