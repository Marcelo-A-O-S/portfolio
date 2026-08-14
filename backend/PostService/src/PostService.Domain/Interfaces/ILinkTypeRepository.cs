using PostService.Domain.Entities;
namespace PostService.Domain.Interfaces
{
    public interface ILinkTypeRepository : IGenerics<LinkType>
    {
        Task<PaginatedResult<LinkType>> GetByPagination(int page, string? search, int itemsPage = 10);
    }
}