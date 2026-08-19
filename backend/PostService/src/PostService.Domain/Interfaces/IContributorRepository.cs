using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Domain.Interfaces
{
    public interface IContributorRepository : IGenerics<Contributor>
    {
        Task<PaginatedResult<ContributorView>> GetByPagination(int page, Guid postId, string? search, int itemsPage = 10);
    }
}