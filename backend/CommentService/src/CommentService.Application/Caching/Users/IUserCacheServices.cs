namespace CommentService.Application.Caching.Users
{
    public interface IUserCacheServices
    {
        Task AddUserCache(string key, Guid userId);
        Task<string?> GetUserCache(string key);
        Task RemoveUserCache(string key);
        Task AddProviderCache(string key, string providerId);
        Task<string?> GetProviderCache(string key);
    }
}