using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IUpdateProject
    {
        Task ExecuteAsync(Guid authenticatedUserId, string role, Guid Id, PostRequest postRequest);
    }
}