using CertificateService.Application.Caching.Interfaces;
using CertificateService.Application.Constants;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
namespace CertificateService.Application.UseCases.Certificates
{
    public class RemoveCertificate : IRemoveCertificate
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IUnitOfWork unitOfWork;
        public RemoveCertificate(
            IMediaProjectionServices _mediaProjectionServices,
            ICertificateServices _certificateServices,
            ICertificateCacheServices _certificateCacheServices,
            IRabbitMQProducer _rabbitMQProducer,
            IUnitOfWork _unitOfWork
        )
        {
            this.mediaProjectionServices = _mediaProjectionServices;
            this.certificateServices = _certificateServices;
            this.certificateCacheServices = _certificateCacheServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid certificateId)
        {
            var certificate = await GetCertificateById(certificateId);
            var mediasToDelete = new List<MediaProjection>();
            if(certificate.MediaProjection != null)
                await ProcessImage(certificate, mediasToDelete);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.certificateServices.DeleteById(certificate.Id);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await DeleteMedias(mediasToDelete);
            await this.certificateCacheServices.RemoveCertificateCache(CacheKeys.CertificateExists(certificateId));
        }
        private async Task<Certificate> GetCertificateById(Guid certificateId)
        {
            var certificate =  await this.certificateServices.GetById(certificateId);
            if(certificate == null)
                throw new NotFoundException("Certificado não encontrado.");
            return certificate;
        }
        private async Task ProcessImage(Certificate certificate, List<MediaProjection> mediasToDelete)
        {
            var media = await this.mediaProjectionServices.GetByUrl(certificate.MediaProjection.Url);
            mediasToDelete.Add(media);
        } 
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                await this.rabbitMQProducer.Publish("CertificateMediaDeleted", new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Certificate"
                });
            }
        }
    }
}