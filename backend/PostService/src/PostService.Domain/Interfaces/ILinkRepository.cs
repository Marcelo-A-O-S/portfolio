using PostService.Domain.Entities;
namespace PostService.Domain.Interfaces
{
    public interface ILinkRepository : IGenerics<Link>
    {
        Task<PaginatedResult<Link>> GetByPagination(int page, string? search, int itemsPage = 10);
    }
}