namespace CommentService.Application.Interfaces
{
    public interface IToolCacheServices
    {
        Task AddToolCache(string key, Guid toolId);
        Task<string?> GetToolCache(string key);
        Task RemoveToolCache(string key);
    }
}