using PostService.Domain.Entities;
using PostService.Domain.Queries;

namespace PostService.Domain.Interfaces
{
    public interface ILanguageRepository : IGenerics<Language>
    {
        Task<PaginatedResult<Language>> GetPagination(int page, string? search, string? code, int itemsPage = 10);
        Task<LanguageView> GetByLanguageView(Guid Id);
    }
}