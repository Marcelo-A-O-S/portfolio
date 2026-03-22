using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface IToolsServices : IServices<Tool>
    {
        Task<PaginatedResult<Tool>> GetByPagination(int page, string? search);
        Task<Tool> GetToolById(Guid Id);
    }
}