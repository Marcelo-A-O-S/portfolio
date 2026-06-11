namespace MediaService.Application.Caching.Certificate
{
    public interface ICertificateCacheServices
    {
        Task AddCertificateCache(string key, Guid certificateId);
        Task<string?> GetCertificateCache(string key);
        Task RemoveCertificateCache(string key);
    }
}