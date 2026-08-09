using PostService.Application.DTOs.Request;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Tools.Interfaces
{
    public interface IUpdateTool
    {
        Task ExecuteAsync(Guid authenticatedUserId, string role, Guid Id, ToolRequest toolRequest);
    }
}