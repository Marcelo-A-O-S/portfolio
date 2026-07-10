using AuthService.Application.DTOs.Response;

namespace AuthService.Application.UseCases.InternalUser.Interfaces
{
    public interface IGetByIdUser
    {
        Task<UserResponse> ExecuteAsync(Guid userId, string providerId);
    }
}