using AuthService.Domain.Entities;
namespace AuthService.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IGenerics<RefreshToken>
    {
        Task<RefreshToken> GetByDeviceId(string deviceId);
        Task DeleteExpiredAsync();
    }
}