namespace CertificateService.Application.Interfaces
{
    public interface ICacheServices
    {
        Task SetAsync(string key, string value, TimeSpan ttl);
        Task<string?> GetAsync(string key);
        Task RemoveAsync(string key);
    }
}