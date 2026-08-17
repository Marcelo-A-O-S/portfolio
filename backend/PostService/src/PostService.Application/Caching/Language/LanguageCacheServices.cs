using PostService.Application.Interfaces;
namespace PostService.Application.Caching.Language
{
    public class LanguageCacheServices : ILanguageCacheServices
    {
        private readonly ICacheService cacheServices;
        public LanguageCacheServices(
            ICacheService _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddLanguageCache(string key, Guid languageId)
        {
            await this.cacheServices.SetAsync(key, languageId.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task<string?> GetLanguageCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }

        public async Task RemoveLanguageCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}