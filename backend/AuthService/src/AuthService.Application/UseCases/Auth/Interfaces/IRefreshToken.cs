using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
namespace AuthService.Application.UseCases.Auth.Interfaces
{
    public interface IRefreshToken
    {
        Task<AuthResponse> ExecuteAsync(RefreshRequest refreshRequest);
    }
}