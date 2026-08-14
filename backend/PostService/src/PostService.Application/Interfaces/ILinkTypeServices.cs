using PostService.Domain.Entities;
namespace PostService.Application.Interfaces
{
    public interface ILinkTypeServices : IServices<LinkType>
    {
        Task<PaginatedResult<LinkType>> GetByPagination(int page, string? search, int itemsPage = 10);
    }
}