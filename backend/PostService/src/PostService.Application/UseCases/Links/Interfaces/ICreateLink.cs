using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Links.Interfaces
{
    public interface ICreateLink
    {
        Task ExecuteAsync(LinkRequest request);
    }
}