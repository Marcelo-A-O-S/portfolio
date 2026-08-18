using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface ICreateProject
    {
        Task ExecuteAsync(Guid authenticatedUserId, string providerId, PostRequest postRequest);
    }
}