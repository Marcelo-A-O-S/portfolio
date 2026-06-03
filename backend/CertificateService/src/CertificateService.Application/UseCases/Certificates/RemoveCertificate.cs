using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Domain.Entities;
namespace CertificateService.Application.UseCases.Certificates
{
    public class RemoveCertificate : IRemoveCertificate
    {
        private readonly ICertificateServices certificateServices;
        private readonly IMediaFilesServices mediaFilesServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        public RemoveCertificate(
            ICertificateServices _certificateServices,
            IMediaFilesServices _mediaFilesServices,
            ICertificateCacheServices _certificateCacheServices
        )
        {
            this.certificateServices = _certificateServices;
            this.mediaFilesServices = _mediaFilesServices;
            this.certificateCacheServices = _certificateCacheServices;
        }
        public async Task ExecuteAsync(Guid certificateId)
        {
            var certificate = await GetCertificateById(certificateId);
            var mediasToDelete = new List<MediaFile>();
            if(certificate.MediaFileId is Guid mediaFileId)
            {
                var media = await this.mediaFilesServices.GetById(mediaFileId);
                if(media != null)
                {
                    mediasToDelete.Add(media);
                }
            }
            await this.certificateServices.DeleteById(certificate.Id);
            await DeleteMedias(mediasToDelete);
            await this.certificateCacheServices.RemoveCertificateCache($"certificate:exists:{certificateId}");
        }
        private async Task<Certificate> GetCertificateById(Guid certificateId)
        {
            var certificate =  await this.certificateServices.GetById(certificateId);
            if(certificate == null)
                throw new NotFoundException("Certificado não encontrado.");
            return certificate;
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