using PostService.Domain.Entities;

namespace PostService.Application.Interfaces
{
    public interface ICategoryServices : IServices<Category>
    {
        Task<PaginatedResult<Category>> GetByPagination(int page, string? language, string? search);
        Task<List<Category>> GetCategoriesByLanguage(string language);
        Task<List<Category>> GetCategories();
    }
}