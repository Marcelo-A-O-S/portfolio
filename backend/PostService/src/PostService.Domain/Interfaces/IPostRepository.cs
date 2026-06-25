using PostService.Domain.Entities;
using PostService.Domain.Queries;

namespace PostService.Domain.Interfaces
{
    public interface IPostRepository : IGenerics<Post>
    {
        Task<PaginatedResult<PostView>> GetByPagination(Guid? authenticatedUserId, int page, string? search, int itemsPage = 10);
        Task<Post> GetPostById(Guid Id);
        Task<Post> GetForUpdate(Guid Id);
        Task<List<Post>> GetPosts();
        Task<Post> GetFullDataById(Guid Id);
        Task<int> GetLikesCountByPostId(Guid postId);
        Task IncrementLikeCount(Guid postId);
        Task IncrementCommentCount(Guid postId);
        Task DecrementLikeCount(Guid postId);
        Task DecrementCommentCount(Guid postId);
        
    }
}