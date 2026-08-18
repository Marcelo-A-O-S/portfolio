using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface IAddLinkTool
    {
        Task ExecuteAsync(LinkRequest request);
    }
}