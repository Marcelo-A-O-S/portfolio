using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Links.Interfaces
{
    public interface IUpdateLink
    {
        Task ExecuteAsync(Guid Id, LinkRequest request);
    }
}