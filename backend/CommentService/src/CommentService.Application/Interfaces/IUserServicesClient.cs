using CommentService.Application.DTOs.Response;
namespace CommentService.Application.Interfaces
{
    public interface IUserServicesClient
    {
        Task<bool> UserExistsAsync(Guid userId);
        Task<bool> ProviderExistsAsync(Guid userId, string providerId);
        Task<UserResponse> GetUserAsync(Guid userId, string providerId);
    }
}