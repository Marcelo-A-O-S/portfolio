using System.Runtime.ConstrainedExecution;
using CertificateService.Application.Caching.Interfaces;
using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validations;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
namespace CertificateService.Application.UseCases.Certificates
{
    public class UpdateCertificate : IUpdateCertificate
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IUnitOfWork unitOfWork;
        public UpdateCertificate(
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
        public async Task ExecuteAsync(Guid certificateId, CertificateRequest request)
        {
            ValidateRequest(request);
            var certificate = await GetCertificateById(certificateId);
            var mediasToDelete = new List<MediaProjection>();
            var mediasToCommit = new List<MediaProjection>();
            await this.unitOfWork.BeginAsync();
            try
            {
                certificate.Update(
                    request.Title,
                    request.Description,
                    request.Institution,
                    request.Status,
                    request.CertificateType,
                    request.IssuerDate,
                    request.CredentialId,
                    request.VerificationUrl,
                    request.WorkloadHours
                );
                if(request.Media != null)
                    await ProcessImage(certificate, request.Media, mediasToCommit, mediasToDelete);
                await this.certificateServices.Update(certificate);
                await DeleteMedias(mediasToDelete);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await PublishMedias(certificate.Id, mediasToCommit, mediasToDelete);
        }
        private static void ValidateRequest(CertificateRequest certificateRequest)
        {
            var validationError = ValidationHelper.Validate(certificateRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task<Certificate> GetCertificateById(Guid certificateId)
        {
            var certificate = await this.certificateServices.GetById(certificateId);
            if (certificate == null)
                throw new NotFoundException("Certificado não encontrado.");
            return certificate;
        }
        private async Task ProcessImage(Certificate certificate, MediaRequest mediaRequest, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var validationError = ValidationHelper.Validate(mediaRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            if (certificate.MediaProjectionId == mediaRequest.Id)
                return;
            if (!mediasToDelete.Any(m => m.MediaId == certificate.MediaProjection.MediaId))
            {
                mediasToDelete.Add(certificate.MediaProjection);
            }
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaRequest.Url);
            if (mediaContent != null)
            {
                if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
                {
                    mediasToCommit.Add(mediaContent);
                }
                certificate.AddMedia(mediaContent.Id);
                return;
            }
            mediaContent = new MediaProjection(mediaRequest.MediaId, mediaRequest.Url);
            mediaContent.GenerateId();
            await this.mediaProjectionServices.Save(mediaContent);
            if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
            {
                mediasToCommit.Add(mediaContent);
            }
            certificate.AddMedia(mediaContent.Id);
        }
        private async Task PublishMedias(Guid certificateId, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                await this.rabbitMQProducer.Publish("CertificateMediaDeleted", new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Certificate"
                });
            }
            foreach (var media in mediasToCommit)
            {
                await this.rabbitMQProducer.Publish("CertificateMediaAttached", new
                {
                    MediaId = media.MediaId,
                    OwnerId = certificateId,
                    OwnerType = "Certificate"
                });
            }
        }
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                if (media.Id != Guid.Empty)
                {
                    await this.mediaProjectionServices.Delete(media);
                }
            }
        }
    }
}