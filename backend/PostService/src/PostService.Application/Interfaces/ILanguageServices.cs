using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Application.Interfaces
{
    public interface ILanguageServices: IServices<Language>
    {
        Task<PaginatedResult<Language>> GetPagination(int page, string? search, string? code);
        Task<LanguageView> GetByLanguageView(Guid Id);
    }
}