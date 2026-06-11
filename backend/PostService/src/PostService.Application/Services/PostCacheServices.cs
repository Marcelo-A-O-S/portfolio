using PostService.Application.Interfaces;
namespace PostService.Application.Services
{
    public class PostCacheServices : IPostCacheServices
    {
        private readonly ICacheService cacheServices;
        public PostCacheServices(
            ICacheService _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddPostCache(string key, Guid postId)
        {
            await this.cacheServices.SetAsync(key, postId.ToString(), TimeSpan.FromMinutes(10));
        }
        public async Task<string?> GetPostCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }
        public async Task RemovePostCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}