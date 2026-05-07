using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface ICreateProject
    {
        Task ExecuteAsync(PostRequest postRequest);
    }
}