namespace PostService.Application.Interfaces
{
    public interface IUserServicesClient
    {
        Task<bool> UserExistsAsync(Guid userId);
    }
}