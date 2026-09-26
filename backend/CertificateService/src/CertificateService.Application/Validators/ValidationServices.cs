using CertificateService.Application.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Constants;
using CertificateService.Application.Caching.Interfaces;
namespace CertificateService.Application.Validators
{
    public class ValidationServices : IValidationServices
    {
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostProjectionServices postProjectionServices;
        public ValidationServices(
            ICertificateServices _certificateServices,
            ICertificateCacheServices _certificateCacheServices,
            IPostServicesClient _postServicesClient,
            IPostCacheServices _postCacheServices,
            IPostProjectionServices _postProjectionServices
        )
        {
            this.certificateServices = _certificateServices;
            this.certificateCacheServices = _certificateCacheServices;
            this.postServicesClient = _postServicesClient;
            this.postCacheServices = _postCacheServices;
            this.postProjectionServices = _postProjectionServices;
        }
        public async Task ValidateCertificateExists(Guid certificateId)
        {
            var certificateCache = await this.certificateCacheServices.GetCertificateCache(CacheKeys.CertificateExists(certificateId));
            if(certificateCache == null)
            {
                var exists = await this.certificateServices.Exists(certificateId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.certificateCacheServices.AddCertificateCache(CacheKeys.CertificateExists(certificateId), certificateId);
            }
        }
        public async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache(CacheKeys.PostExists(postId));
            if(postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache(CacheKeys.PostExists(postId), postId);
            }
        }
        public Task ValidatePostProjectionExists(Guid postProjectionId)
        {
            
            throw new NotImplementedException();
        }
    }
}