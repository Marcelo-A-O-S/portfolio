using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;

namespace AuthService.Application.UseCases.Auth.Interfaces
{
    public interface ILogin
    {
        Task<AuthResponse> ExecuteASync(LoginRequest loginRequest);
    }
}