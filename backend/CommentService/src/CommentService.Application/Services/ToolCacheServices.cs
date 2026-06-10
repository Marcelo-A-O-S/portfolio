using CommentService.Application.Interfaces;

namespace CommentService.Application.Services
{
    public class ToolCacheServices : IToolCacheServices
    {
        private readonly ICacheServices cacheServices;
        public ToolCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddToolCache(string key, Guid toolId)
        {
            await this.cacheServices.SetAsync(key, toolId.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task<string?> GetToolCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }

        public async Task RemoveToolCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}