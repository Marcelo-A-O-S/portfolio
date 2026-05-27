namespace CommentService.Application.Interfaces
{
    public interface IUserServicesClient
    {
        Task<bool> UserExistsAsync(Guid userId);
    }
}