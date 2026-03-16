using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface ILanguageServices: IServices<Language>
    {
        Task<PaginatedResult<Language>> GetPagination(int page, string? search, string? code);
    }
}