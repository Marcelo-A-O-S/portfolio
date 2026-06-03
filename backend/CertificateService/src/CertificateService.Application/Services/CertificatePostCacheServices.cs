using CertificateService.Application.Interfaces;
namespace CertificateService.Application.Services
{
    public class CertificatePostCacheServices : ICertificatePostsCacheServices
    {
        private readonly ICacheServices cacheServices;
        public CertificatePostCacheServices(
            ICacheServices _cacheServices
        )
        {
            this.cacheServices = _cacheServices;
        }
        public async Task AddCertificatePostCache(string key, Guid certificatePostId)
        {
            await this.cacheServices.SetAsync(key, certificatePostId.ToString(), TimeSpan.FromMinutes(10));
        }

        public async Task<string?> GetCertificatePostCache(string key)
        {
            return await this.cacheServices.GetAsync(key);
        }

        public async Task RemoveCertificatePostCache(string key)
        {
            await this.cacheServices.RemoveAsync(key);
        }
    }
}