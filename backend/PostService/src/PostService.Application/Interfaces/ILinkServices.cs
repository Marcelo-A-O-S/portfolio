using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Application.Interfaces
{
    public interface ILinkServices : IServices<Link>
    {
        Task<PaginatedResult<LinkView>> GetByPagination(int page, Guid? toolId, Guid? postId, string? search, int itemsPage = 10);
        Task<Link> GetFullDataById(Guid Id);
    }
}