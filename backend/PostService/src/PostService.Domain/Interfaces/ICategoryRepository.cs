using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface ICategoryRepository : IGenerics<Category>
    {
        Task<PaginatedResult<Category>> GetByPagination(int page, string? language, string? search, int itemsPage = 10);
    }
}