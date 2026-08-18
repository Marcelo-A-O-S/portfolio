using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IAddLinkProject
    {
        Task ExecuteAsync(LinkRequest request);
    }
}