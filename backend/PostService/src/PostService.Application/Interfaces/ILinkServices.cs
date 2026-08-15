using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface ILinkServices : IServices<Link>
    {
        Task<PaginatedResult<Link>> GetByPagination(int page, string? search, int itemsPage = 10);
    }
}