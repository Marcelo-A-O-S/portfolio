using AuthService.Application.DTOs.Response;
using AuthService.Domain.Entities;
namespace AuthService.Application.Interfaces
{
    public interface IJwtBearerServices
    {
        Task<(string token, int expireIn)> GenerateAccessToken(User user);
        Task<(RefreshToken entity, string plainToken)> GenerateRefreshToken(Guid userId, string deviceId, string deviceName);
        Task<AuthResponse> RefreshAsync(Guid refreshTokenId, Guid userId, string refreshToken, string deviceId, string deviceName);
    }
}