namespace PostService.Application.Validators.Interfaces
{
    public interface IToolValidationServices
    {
        Task ValidateProviderExists(Guid userId, string providerId);
        Task ValidateUserExists(Guid userId);
        Task ValidatePostExists(Guid postId);
        Task ValidateToolExists(Guid toolId);
    }
}