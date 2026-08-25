using CertificateService.Application.Interfaces;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Constants;

namespace CertificateService.Application.Validators
{
    public class ValidationServices : IValidationServices
    {
        private readonly IPostServicesClient postServicesClient;
        private readonly IPostCacheServices postCacheServices;
        public ValidationServices(
            IPostServicesClient _postServicesClient,
            IPostCacheServices _postCacheServices
        )
        {
            this.postServicesClient = _postServicesClient;
            this.postCacheServices = _postCacheServices;
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
    }
}