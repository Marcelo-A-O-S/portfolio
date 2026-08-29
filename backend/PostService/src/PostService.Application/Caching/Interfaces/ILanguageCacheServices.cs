namespace PostService.Application.Caching.Interfaces
{
    public interface ILanguageCacheServices
    {
        Task AddLanguageCache(string key, Guid languageId);
        Task<string?> GetLanguageCache(string key);
        Task RemoveLanguageCache(string key);
    }
}