using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface IPostRepository : IGenerics<Post>
    {
        Task<PaginatedResult<Post>> GetByPagination(int page, string? search, int itemsPage = 10);
        
    }
}