namespace CommentService.Application.Interfaces
{
    public interface IUserCacheServices
    {
        Task AddUserCache(string key, Guid userId);
        Task<string?> GetUserCache(string key);
        Task RemoveUserCache(string key);
    }
}