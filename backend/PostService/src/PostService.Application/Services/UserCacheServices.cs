using PostService.Application.Interfaces;

namespace PostService.Application.Services
{
    public class UserCacheServices : IUserCacheServices
    {
        private readonly ICacheService cacheService;
        public UserCacheServices(
            ICacheService _cacheService
        )
        {
            this.cacheService = _cacheService;
        }
        public async Task AddUserCache(string key, Guid userId)
        {
            await this.cacheService.SetAsync(key, userId.ToString(), TimeSpan.FromMinutes(10));
        }
        public async Task<string?> GetUserCache(string key)
        {
            return await this.cacheService.GetAsync(key);
        }
        public async Task RemoveUserCache(string key)
        {
            await this.cacheService.RemoveAsync(key);
        }
    }
}