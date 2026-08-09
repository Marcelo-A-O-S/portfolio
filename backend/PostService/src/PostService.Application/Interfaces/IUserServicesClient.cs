using PostService.Application.DTOs.Response;
namespace PostService.Application.Interfaces
{
    public interface IUserServicesClient
    {
        Task<bool> UserExistsAsync(Guid userId);
        Task<bool> ProviderExistsAsync(Guid userId, string providerId);
        Task<UserResponse> GetUserAsync(Guid userId, string providerId);
    }
}