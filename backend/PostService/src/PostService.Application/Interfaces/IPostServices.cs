using PostService.Domain.Entities;

namespace PostService.Application.Interfaces
{
    public interface IPostServices : IServices<Post>
    {
        Task<PaginatedResult<Post>> GetByPagination(int page, string? search);
    }
}