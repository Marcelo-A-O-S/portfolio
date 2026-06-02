using System.ComponentModel.DataAnnotations;
using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validations;
using CertificateService.Domain.Entities;

namespace CertificateService.Application.UseCases.Certificates
{
    public class AddCertificate : IAddCertificate
    {
        private readonly IMediaFilesServices mediaFilesServices;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        public AddCertificate(
            IMediaFilesServices _mediaFilesServices,
            ICertificateServices _certificateServices,
            ICertificateCacheServices _certificateCacheServices
        )
        {
            this.mediaFilesServices = _mediaFilesServices;
            this.certificateServices = _certificateServices;
            this.certificateCacheServices = _certificateCacheServices;
        }
        public async Task ExecuteAsync(CertificateRequest certificateRequest)
        {
            ValidateRequest(certificateRequest);
            var certificate = new Certificate(
                certificateRequest.Title, 
                certificateRequest.Description, 
                certificateRequest.Institution,
                certificateRequest.Status
            );
            var media = await this.mediaFilesServices.SaveImageAsync(certificateRequest.ImgFile!, "media/certificates");
            certificate.AddImgUrl(media!.Path, media.Id);
            await this.certificateServices.Save(certificate);
            await this.mediaFilesServices.Save(media);
            await this.certificateCacheServices.AddCertificateCache($"", certificate.Id);
        }
        private static void ValidateRequest(CertificateRequest certificateRequest)
        {
            var validationError = ValidationHelper.Validate(certificateRequest);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task ValidatePostExists(Guid postId)
        {
            
        }
    }
}