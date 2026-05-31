using CommentService.Application.Interfaces;
namespace CommentService.Application.Services
{
    public class LikeCacheServices : ILikeCacheServices
    {
        private readonly ICacheServices cacheServices;
        public LikeCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddLikeCache(string key, Guid likeId)
        {
            await this.cacheServices.SetAsync(key, likeId.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task<string?> GetLikeCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }

        public async Task RemoveLikeCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}