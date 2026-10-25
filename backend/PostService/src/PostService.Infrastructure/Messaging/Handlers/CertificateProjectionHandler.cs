using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class CertificateProjectionHandler : ICertificateProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CertificateProjectionHandler> logger;
        public CertificateProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<CertificateProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleCertificateAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateAddedEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            var languageServices = scope.ServiceProvider.GetRequiredService<ILanguageServices>();
            if (payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindBy(cp => cp.PostId == payload.PostId && cp.CertificateId == payload.CertificateId);
            if (certificateProjection != null)
            {
                this.logger.LogWarning("Evento recebido para certificado já associado. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            certificateProjection = new CertificateProjection(
                    payload.CertificateId,
                    payload.PostId
                );
            foreach (var dataContent in payload.CertificateContents)
            {
                if (dataContent.Language?.Id is not Guid languageId)
                {
                    this.logger.LogWarning("Evento de certificado sem Language.Id válido. CertificateId: {CertificateId}", payload.CertificateId);
                    return;
                }
                var language = await languageServices.GetById(languageId);
                if (language == null)
                {
                    this.logger.LogWarning("Idioma {LanguageId} não encontrado localmente para o certificado {CertificateId}", languageId, payload.CertificateId);
                    return;
                }     
                var certificateContentProjection = new CertificateContentProjection(
                    dataContent.Title,
                    languageId
                );
                certificateProjection.AddCertificateContentProjection(certificateContentProjection);
            }
            await unitOfWork.BeginAsync();
            try
            {
                await certificateProjectionServices.Save(certificateProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandleCertificateRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateRemoveEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            if (payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindBy(cp => cp.PostId == payload.PostId && cp.CertificateId == payload.CertificateId);
            if (certificateProjection == null)
            {
                this.logger.LogWarning("Evento recebido para certificado inexistente. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            await unitOfWork.BeginAsync();
            try
            {
                await certificateProjectionServices.Delete(certificateProjection);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}