using AuthService.Application.DTOs.Request;

namespace AuthService.Application.UseCases.Auth.Interfaces
{
    public interface ILogout
    {
        Task ExecuteAsync(LogoutRequest logoutRequest);
    }
}