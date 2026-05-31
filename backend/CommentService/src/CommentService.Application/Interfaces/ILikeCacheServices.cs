namespace CommentService.Application.Interfaces
{
    public interface ILikeCacheServices
    {
        Task AddLikeCache(string key, Guid likeId);
        Task<string?> GetLikeCache(string key);
        Task RemoveLikeCache(string key);
    }
}