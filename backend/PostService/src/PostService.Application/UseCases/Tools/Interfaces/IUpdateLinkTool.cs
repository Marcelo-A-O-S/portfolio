using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface IUpdateLinkTool
    {
        Task ExecuteAsync(Guid Id,LinkRequest request);
    }
}