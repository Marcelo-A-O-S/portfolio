using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;

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
            if(payload == null)
                return;
            
            throw new NotImplementedException();
        }

        public async Task HandleCertificateRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<CertificateRemoveEvent>(message);
            using var scope = this.scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var certificateProjectionServices = scope.ServiceProvider.GetRequiredService<ICertificateProjectionServices>();
            if(payload == null)
                return;
            var certificateProjection = await certificateProjectionServices.FindBy(cp => cp.PostId == payload.PostId && cp.CertificateId == payload.CertificateId);
            if(certificateProjection == null)
            {
                this.logger.LogWarning("Evento recebido para certificado inexistente. CertificateId: {CertificateId}", payload.CertificateId);
                return;
            }
            await unitOfWork.BeginAsync();
            try{
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