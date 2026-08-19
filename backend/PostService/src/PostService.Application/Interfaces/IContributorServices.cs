using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Application.Interfaces
{
    public interface IContributorServices : IServices<Contributor>
    {
        Task<PaginatedResult<ContributorView>> GetByPagination(int page, Guid postId, string? search, int itemsPage = 10);
    }
}