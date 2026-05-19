using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IUpdateProject
    {
        Task ExecuteAsync(Guid Id, PostRequest postRequest);
    }
}