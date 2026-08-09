using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface ICreateTool
    {
        Task ExecuteAsync(Guid authenticatedUserId, string provider, ToolRequest request);
    }
}