using PostService.Domain.Entities;
namespace PostService.Domain.Interfaces
{
    public interface IToolsRepository : IGenerics<Tool>
    {
        Task<PaginatedResult<Tool>> GetByPagination(int page, string? search, int itemsPage = 10);
        Task<Tool> GetToolById(Guid Id);
        Task<Tool> GetForUpdate(Guid Id);
    }
}