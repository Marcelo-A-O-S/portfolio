using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface IPostRepository : IGenerics<Post>
    {
        Task<PaginatedResult<Post>> GetByPagination(int page, string? search, int itemsPage = 10);
        Task<Post> GetPostById(Guid Id);
        Task<Post> GetForUpdate(Guid Id);

    }
}