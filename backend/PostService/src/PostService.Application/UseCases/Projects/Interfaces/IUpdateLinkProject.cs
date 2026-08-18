using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Projects.Interfaces
{
    public interface IUpdateLinkProject
    {
        Task ExecuteAsync(Guid Id, LinkRequest request);
    }
}