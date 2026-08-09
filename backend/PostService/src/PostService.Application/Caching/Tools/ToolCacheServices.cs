using PostService.Application.Interfaces;
namespace PostService.Application.Caching.Tools
{
    public class ToolCacheServices : IToolCacheServices
    {
        private readonly ICacheService cacheServices;
        public ToolCacheServices(
            ICacheService _cacheServices
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