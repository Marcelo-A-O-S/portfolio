using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Domain.Interfaces
{
    public interface IToolsRepository : IGenerics<Tool>
    {
        Task<PaginatedResult<ToolView>> GetByPagination(Guid? authenticatedUserId, int page, string? search, int itemsPage = 10);
        Task<Tool> GetToolById(Guid Id);
        Task<Tool> GetForUpdate(Guid Id);
        Task<List<Tool>> GetTools();
        Task<Tool> GetFullDataById(Guid Id);
        Task IncrementLikeCount(Guid Id);
        Task IncrementCommentCount(Guid Id);
        Task DecrementLikeCount(Guid Id);
        Task DecrementCommentCount(Guid Id);
    }
}