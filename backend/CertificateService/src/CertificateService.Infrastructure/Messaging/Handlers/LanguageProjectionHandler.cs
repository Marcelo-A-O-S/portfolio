using System.Text.Json;
using CertificateService.Application.Interfaces;
using CertificateService.Infrastructure.Messaging.Events;
using CertificateService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CertificateService.Infrastructure.Messaging.Handlers
{
    public class LanguageProjectionHandler : ILanguageProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<LanguageProjectionHandler> logger;
        public LanguageProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<LanguageProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleLanguageDeleted(string message)
        {
            var payload = JsonSerializer.Deserialize<LanguageDeletedEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var languageProjectionServices = scope.ServiceProvider.GetRequiredService<ILanguageProjectionServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var languageProjection = await languageProjectionServices.GetByLanguageId(payload.LanguageId);
            if(languageProjection == null)
            {
                this.logger.LogWarning("Evento recebido para idioma inexistente. LanguageId: {LanguageId}", payload.LanguageId);
                return;
            }
            await unitOfWork.BeginAsync();
            try
            {
                await languageProjectionServices.DeleteByLanguageId(languageProjection.LanguageId);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandleLanguageUpdated(string message)
        {
            var payload = JsonSerializer.Deserialize<LanguageUpdatedEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var languageProjectionServices = scope.ServiceProvider.GetRequiredService<ILanguageProjectionServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var languageProjection = await languageProjectionServices.GetByLanguageId(payload.LanguageId);
            if(languageProjection == null)
            {
                this.logger.LogWarning("Evento recebido para idioma inexistente. LanguageId: {LanguageId}", payload.LanguageId);
                return;
            }
            languageProjection.Update(payload.Code, payload.Name);
            await unitOfWork.BeginAsync();
            try
            {
                await languageProjectionServices.Update(languageProjection);
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