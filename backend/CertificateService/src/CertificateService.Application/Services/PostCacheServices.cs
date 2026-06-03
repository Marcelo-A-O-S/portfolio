using CertificateService.Application.Interfaces;

namespace CertificateService.Application.Services
{
    public class PostCacheServices : IPostCacheServices
    {
        private readonly ICacheServices cacheServices;
        public PostCacheServices(
            ICacheServices _cacheServices
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