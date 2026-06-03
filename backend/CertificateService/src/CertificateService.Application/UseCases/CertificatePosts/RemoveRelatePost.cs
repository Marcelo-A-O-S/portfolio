using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.CertificatePosts.Interfaces;
using CertificateService.Domain.Entities;
namespace CertificateService.Application.UseCases.CertificatePosts
{
    public class RemoveRelatePost : IRemoveRelatePost
    {
        private readonly ICertificatePostsServices certificatePostsServices;
        private readonly ICertificatePostsCacheServices certificatePostsCacheServices;
        public RemoveRelatePost(
            ICertificatePostsServices _certificatePostsServices,
            ICertificatePostsCacheServices _certificatePostsCacheServices
        )
        {
            this.certificatePostsServices = _certificatePostsServices;
            this.certificatePostsCacheServices = _certificatePostsCacheServices;
        }
        public async Task ExecuteAsync(Guid certificatePostId)
        {
            var certificatePost = await GetCertificatePostAsync(certificatePostId);
            await this.certificatePostsServices.DeleteById(certificatePost.Id);
            await this.certificatePostsCacheServices.RemoveCertificatePostCache($"certificatePost:exists:{certificatePostId}");
        }
        private async Task<CertificatePost> GetCertificatePostAsync(Guid certificatePostId)
        {
            var certificatePost = await this.certificatePostsServices.GetById(certificatePostId);
            if(certificatePost == null)
                throw new NotFoundException("Relacionamento não encontrado");
            return certificatePost;
        }
    }
}