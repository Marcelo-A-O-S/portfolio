namespace PostService.Application.Interfaces
{
    public interface ILikeCacheServices
    {
        Task AddLikeCache(string key, bool status);
        Task<string?> GetLikeCache(string key);
        Task RemoveLikeCache(string key);
    }
}