using CommentService.Application.Interfaces;
namespace CommentService.Application.Services
{
    public class CommentCacheServices : ICommentCacheServices
    {
        private readonly ICacheServices cacheServices;
        public CommentCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddCommentCache(string key, Guid commentId)
        {
            await this.cacheServices.SetAsync(key, commentId.ToString(), TimeSpan.FromMinutes(10));
        }
        public async Task<string?> GetCommentCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }
        public async Task RemoveCommentCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}