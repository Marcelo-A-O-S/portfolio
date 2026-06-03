using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.CertificatePosts.Interfaces;
using CertificateService.Domain.Entities;
namespace CertificateService.Application.UseCases.CertificatePosts
{
    public class AddRelatePost : IAddRelatePost
    {
        private readonly IPostServicesClient postServicesClient;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly ICertificatePostsServices certificatePostsServices;
        private readonly ICertificatePostsCacheServices certificatePostsCacheServices;
        private readonly IPostCacheServices postCacheServices;
        public AddRelatePost(
            IPostServicesClient _postServicesClient,
            ICertificateServices _certificateServices,
            ICertificateCacheServices _certificateCacheServices,
            ICertificatePostsServices _certificatePostsServices,
            ICertificatePostsCacheServices _certificatePostsCacheServices,
            IPostCacheServices _postCacheServices
        )
        {
            this.postServicesClient = _postServicesClient;
            this.certificateServices = _certificateServices;
            this.certificateCacheServices = _certificateCacheServices;
            this.certificatePostsServices = _certificatePostsServices;
            this.certificatePostsCacheServices = _certificatePostsCacheServices;
            this.postCacheServices = _postCacheServices;
        }
        public async Task ExecuteAsync(Guid certificateId, Guid postId)
        {
            await ValidatePostExists(postId);
            await ValidateCertificateExists(certificateId);
            var relatePost = new CertificatePost(
                certificateId,
                postId
            );
            await this.certificatePostsServices.Save(relatePost);
            await this.certificatePostsCacheServices.AddCertificatePostCache($"certificatePost:exists:{relatePost.Id}", relatePost.Id);
        }
        private async Task ValidatePostExists(Guid postId)
        {
            var existsCache = await this.postCacheServices.GetPostCache($"certificate:post:exists:{postId}");
            if (existsCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache($"certificate:post:exists:{postId}", postId);
            }
        }
        private async Task ValidateCertificateExists(Guid certificateId)
        {
            var existsCache = await this.certificateCacheServices.GetCertificateCache($"certificate:exists:{certificateId}");
            if (existsCache == null)
            {
                var exists = await this.certificateServices.GetById(certificateId);
                if (exists == null)
                    throw new NotFoundException("Certificado não encontrado");
                await this.certificateCacheServices.AddCertificateCache($"certificate:exists:{certificateId}", certificateId);
            }
        }
    }
}