namespace CommentService.Application.Interfaces
{
    public interface ICommentCacheServices
    {
        Task AddCommentCache(string key, Guid commentId);
        Task<string?> GetCommentCache(string key);
        Task RemoveCommentCache(string key);
    }
}