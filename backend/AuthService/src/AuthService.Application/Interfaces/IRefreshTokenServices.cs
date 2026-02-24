using AuthService.Domain.Entities;
namespace AuthService.Application.Interfaces
{
    public interface IRefreshTokenServices : IServices<RefreshToken>
    {
        Task CleanupExpiredTokensAsync();
    }
}