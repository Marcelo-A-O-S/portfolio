namespace CommentService.Application.Caching.Like
{
    public interface ILikeCacheServices
    {
        Task AddLikeCache(string key, Guid likeId);
        Task<string?> GetLikeCache(string key);
        Task RemoveLikeCache(string key);
    }
}