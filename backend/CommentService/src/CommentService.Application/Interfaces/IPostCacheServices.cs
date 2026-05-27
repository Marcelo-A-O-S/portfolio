namespace CommentService.Application.Interfaces
{
    public interface IPostCacheServices
    {
        Task AddPostCache(string key, Guid postId);
        Task<string?> GetPostCache(string key);
        Task RemovePostCache(string key);
    }
}