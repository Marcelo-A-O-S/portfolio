using CommentService.Application.Interfaces;

namespace CommentService.Application.Services
{
    public class UserCacheServices : IUserCacheServices
    {
        private readonly ICacheServices cacheServices;
        public UserCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddUserCache(string key, Guid userId)
        {
            await this.cacheServices.SetAsync(key, userId.ToString(), TimeSpan.FromMinutes(10));
        }
        public async Task<string?> GetUserCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }
        public async Task RemoveUserCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}