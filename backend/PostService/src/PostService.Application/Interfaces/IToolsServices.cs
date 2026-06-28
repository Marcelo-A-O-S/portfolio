using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Application.Interfaces
{
    public interface IToolsServices : IServices<Tool>
    {
        Task<PaginatedResult<ToolView>> GetByPagination(Guid? authenticatedUserId, int page, string? search);
        Task<ToolView> GetToolById(Guid? authenticatedUserId, Guid Id);
        Task<Tool> GetForUpdate(Guid Id);
        Task<List<ToolView>> GetTools();
        Task<Tool> GetFullDataById(Guid Id);
        Task IncrementLikeCount(Guid Id);
        Task IncrementCommentCount(Guid Id);
        Task DecrementLikeCount(Guid Id);
        Task DecrementCommentCount(Guid Id);
    }
}