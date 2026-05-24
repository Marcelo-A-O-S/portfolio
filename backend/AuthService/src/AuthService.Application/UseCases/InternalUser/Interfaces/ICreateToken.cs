using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;

namespace AuthService.Application.UseCases.InternalUser.Interfaces
{
    public interface ICreateToken
    {
        Task<TokenResponse> ExecuteAsync(ServiceAuthRequest request);
    }
}