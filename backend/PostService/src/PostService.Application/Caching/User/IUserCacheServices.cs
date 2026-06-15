namespace PostService.Application.Caching.User
{
    public interface IUserCacheServices
    {
        Task AddUserCache(string key, Guid userId);
        Task<string?> GetUserCache(string key);
        Task RemoveUserCache(string key);
    }
}