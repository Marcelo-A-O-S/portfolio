using MediaService.Application.Caching.Certificate;
using MediaService.Application.Caching.Post;
using MediaService.Application.Caching.Tool;
using MediaService.Application.Constants;
using MediaService.Application.Exceptions;
using MediaService.Application.Interfaces;
using MediaService.Application.Validators.Interfaces;
using MediaService.Domain.Entities;
namespace MediaService.Application.Validators
{
    public class MediaValidationServices : IMediaValidationServices
    {
        private readonly IPostCacheServices postCacheServices;
        private readonly IToolCacheServices toolCacheServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IToolServicesClient toolServicesClient;
        private readonly ICertificateServicesClient certificateServicesClient;
        public MediaValidationServices(
            IPostCacheServices _postCacheServices,
            IToolCacheServices _toolCacheServices,
            ICertificateCacheServices _certificateCacheServices,
            IPostServicesClient _postServicesClient,
            IToolServicesClient _toolServicesClient,
            ICertificateServicesClient _certificateServicesClient
        )
        {
            this.postCacheServices = _postCacheServices;
            this.toolCacheServices = _toolCacheServices;
            this.certificateCacheServices = _certificateCacheServices;
            this.postServicesClient = _postServicesClient;
            this.toolServicesClient = _toolServicesClient;
            this.certificateServicesClient = _certificateServicesClient;
        }
        public async Task ValidateCertificateExists(Guid certificateId)
        {
            var certificateCache = await this.certificateCacheServices.GetCertificateCache(CacheKeys.CertificateExists(certificateId));
            if(certificateCache == null)
            {
                var exists = await this.certificateServicesClient.CertificateExistsAsync(certificateId);
                if (!exists)
                    throw new NotFoundException("Certificado não encontrado");
                await this.certificateCacheServices.AddCertificateCache(CacheKeys.CertificateExists(certificateId), certificateId);
            }
        }
        public async Task ValidateOwnerExists(Guid ownerId, string ownerType)
        {
            switch (ownerType)
            {
                case OwnerTypes.Post:
                    await ValidatePostExists(ownerId);
                    break;
                case OwnerTypes.Tool:
                    await ValidateToolExists(ownerId);
                    break;
                case OwnerTypes.Certificate:
                    await ValidateCertificateExists(ownerId);
                    break;
                default:
                    throw new ValidationException("Tipo de imagem de arquivo inválida.");
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
        public async Task ValidateToolExists(Guid toolId)
        {
            var toolCache = await this.toolCacheServices.GetToolCache(CacheKeys.ToolExists(toolId));
            if(toolCache == null)
            {
                var exists = await this.toolServicesClient.ToolExistsAsync(toolId);
                if(!exists)
                    throw new NotFoundException("Ferramenta não encontrada");
                await this.toolCacheServices.AddToolCache(CacheKeys.ToolExists(toolId), toolId);
            }
        }
    }
}