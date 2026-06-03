
using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.Exceptions;
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
            var mediasToCommit = new List<MediaFile>();
            var certificate = new Certificate(
                certificateRequest.Title, 
                certificateRequest.Description, 
                certificateRequest.Institution,
                certificateRequest.Status,
                certificateRequest.CertificateType,
                certificateRequest.IssueDate,
                certificateRequest.CredentialId,
                certificateRequest.VerificationUrl,
                certificateRequest.WorkloadHours
            );
            var media = await this.mediaFilesServices.SaveImageAsync(certificateRequest.ImgFile!, "media/certificates");
            certificate.AddImgUrl(media!.Path, media.Id);
            mediasToCommit.Add(media);
            await this.certificateServices.Save(certificate);
            await CommitMedias(mediasToCommit);
            await this.certificateCacheServices.AddCertificateCache($"certificate:exists:{certificate.Id}", certificate.Id);
        }
        private static void ValidateRequest(CertificateRequest certificateRequest)
        {
            if(certificateRequest.ImgFile == null)
                throw new ValidationException("O arquivo de imagem é obrigatório.");
            var validationError = ValidationHelper.Validate(certificateRequest);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task CommitMedias(List<MediaFile> mediasToCommit)
        {
            foreach (var media in mediasToCommit)
            {
                media.Commit();
                await this.mediaFilesServices.Update(media);
            }
        }
    }
}