
using MediaService.Application.Interfaces;
namespace MediaService.Application.Caching.Certificate
{
    public class CertificateCacheServices : ICertificateCacheServices
    {
        private readonly ICacheServices cacheServices;
        public CertificateCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddCertificateCache(string key, Guid certificateId)
        {
            await this.cacheServices.SetAsync(key, certificateId.ToString(), TimeSpan.FromMinutes(10));
        }
        public async Task<string?> GetCertificateCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }
        public async Task RemoveCertificateCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}