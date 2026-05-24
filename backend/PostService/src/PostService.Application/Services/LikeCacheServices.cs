using PostService.Application.Interfaces;

namespace PostService.Application.Services
{
    public class LikeCacheServices : ILikeCacheServices
    {
        private readonly ICacheService cacheServices;
        public LikeCacheServices(
            ICacheService _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddLikeCache(string key, bool status)
        {
            await this.cacheServices.SetAsync(key, true.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task<string?> GetLikeCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }

        public Task RemoveLikeCache(string key)
        {
            throw new NotImplementedException();
        }
    }
}