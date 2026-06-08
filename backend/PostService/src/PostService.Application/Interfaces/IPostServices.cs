using PostService.Domain.Entities;
using PostService.Domain.Queries;
namespace PostService.Application.Interfaces
{
    public interface IPostServices : IServices<Post>
    {
        Task<PaginatedResult<PostView>> GetByPagination(int page, string? search);
        Task<Post> GetPostById(Guid Id);
        Task<Post> GetForUpdate(Guid Id);
        Task<List<Post>> GetPosts();
        Task<Post> GetFullDataById(Guid Id);
        Task<int> GetLikesCountByPostId(Guid postId);
    }
}