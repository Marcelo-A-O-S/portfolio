
using System.Runtime.ConstrainedExecution;
using CertificateService.Application.Constants;
using CertificateService.Application.DTOs.Requests;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Application.Validations;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
namespace CertificateService.Application.UseCases.Certificates
{
    public class AddCertificate : IAddCertificate
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly ICertificateServices certificateServices;
        private readonly ICertificateCacheServices certificateCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IUnitOfWork unitOfWork;
        public AddCertificate(
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
        public async Task ExecuteAsync(CertificateRequest request)
        {
            ValidateRequest(request);
            var mediasToCommit = new List<MediaProjection>();
            var certificate = new Certificate(
                request.Title,
                request.Description,
                request.Institution,
                request.CredentialId,
                request.VerificationUrl,
                request.WorkloadHours,
                request.Status,
                request.CertificateType,
                request.IssueDate
            );
            await this.unitOfWork.BeginAsync();
            try
            {
                await ProcessImage(certificate, request.Media, mediasToCommit);
                await this.certificateServices.Save(certificate);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await PublishMedias(certificate.Id, mediasToCommit);
            await this.certificateCacheServices.AddCertificateCache(CacheKeys.CertificateExists(certificate.Id), certificate.Id);
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
        private async Task ProcessImage(Certificate certificate, MediaRequest mediaRequest, List<MediaProjection> mediasToCommit)
        {
            var validationError = ValidationHelper.Validate(mediaRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaRequest.Url);
            if (mediaContent != null)
            {
                if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
                {
                    mediasToCommit.Add(mediaContent);
                }
                certificate.AddImgUrl(mediaContent.Id);
                return;
            }
            mediaContent = new MediaProjection(mediaRequest.MediaId, mediaRequest.Url);
            mediaContent.GenerateId();
            await this.mediaProjectionServices.Save(mediaContent);
            if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
            {
                mediasToCommit.Add(mediaContent);
            }
            certificate.AddImgUrl(mediaContent.Id);
        }
        private async Task PublishMedias(Guid certificateId, List<MediaProjection> mediasToCommit)
        {
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
    }
}