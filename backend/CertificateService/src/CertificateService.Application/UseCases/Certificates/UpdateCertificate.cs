using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validations;
using CertificateService.Domain.Entities;
using CertificateService.Application.Exceptions;
namespace CertificateService.Application.UseCases.Certificates
{
    public class UpdateCertificate : IUpdateCertificate
    {
        private readonly IMediaFilesServices mediaFilesServices;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        public UpdateCertificate(
            IMediaFilesServices _mediaFilesServices,
            ICertificateServices _certificateServices,
            ICertificateCacheServices _certificateCacheServices
        )
        {
            this.mediaFilesServices = _mediaFilesServices;
            this.certificateServices = _certificateServices;
            this.certificateCacheServices = _certificateCacheServices;
        }
        public async Task ExecuteAsync(Guid certificateId, CertificateRequest certificateRequest)
        {
            ValidateRequest(certificateRequest);
            var certificate = await GetCertificateById(certificateId);
            var mediasToDelete = new List<MediaFile>();
            var mediasToCommit = new List<MediaFile>();
            certificate.Update(
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
            await UpdateImageCertificate(certificate, certificateRequest, mediasToCommit, mediasToDelete);
            await this.certificateServices.Update(certificate);
            await CommitMedias(mediasToCommit);
            await DeleteMedias(mediasToDelete);
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
        private async Task<Certificate> GetCertificateById(Guid certificateId)
        {
            var certificate =  await this.certificateServices.GetById(certificateId);
            if(certificate == null)
                throw new NotFoundException("Certificado não encontrado.");
            return certificate;
        }
        private async Task UpdateImageCertificate(Certificate certificate, CertificateRequest request, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            if(certificate.ImgUrl != request.ImgUrl)
            {
                if(certificate.MediaFileId is Guid mediaFileId)
                {
                    var media = await this.mediaFilesServices.GetById(mediaFileId);
                    if(media != null)
                    {
                        mediasToDelete.Add(media);
                    }
                }
                if(request.ImgFile == null)
                    throw new ValidationException("O arquivo de imagem é obrigatório.");
                var newMedia = await this.mediaFilesServices.SaveImageAsync(request.ImgFile!, "media/certificates");
                certificate.AddImgUrl(newMedia!.Path, newMedia.Id);
                mediasToCommit.Add(newMedia);
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
        private async Task DeleteMedias(List<MediaFile> mediasToDelete)
        {
            foreach (var mediaDelete in mediasToDelete)
            {
                await this.mediaFilesServices.DeleteImageAsync(mediaDelete);
            }
        }
    }
}