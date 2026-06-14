using System.Text.Json;
using MediaService.Application.Interfaces;
using MediaService.Application.Validators.Interfaces;
using MediaService.Infrastructure.Messaging.Events;
using MediaService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace MediaService.Infrastructure.Messaging.Handlers
{
    public class MediaFileProjectionHandler : IMediaFileProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<MediaFileProjectionHandler> logger;
        public MediaFileProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<MediaFileProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleMediaCommit(string message)
        {
            var payload = JsonSerializer.Deserialize<MediaCommitEvent>(message);
            if(payload == null)
            {
                this.logger.LogError("Erro ao deserializar payload de evento de commit de midia.");
                return;
            }
            using var scope = this.scopeFactory.CreateScope();
            var mediaFileServices = scope.ServiceProvider.GetRequiredService<IMediaFileServices>();
            var mediaValidationServices = scope.ServiceProvider.GetRequiredService<IMediaValidationServices>();
            var media = await mediaFileServices.FindBy(m => m.Id == payload.MediaFileId && m.OwnerType == payload.OwnerType);
            if(media == null)
            {
                this.logger.LogError("Midia não encontrada.");
                return;
            }
            await mediaValidationServices.ValidateOwnerExists(payload.OwnerId, payload.OwnerType);
            media.AssignOwner(payload.OwnerId);
            media.Commit();
            try
            {
                await mediaFileServices.Update(media);
                this.logger.LogInformation("Midia commitada com sucesso");
            }
            catch(Exception ex)
            {
                logger.LogError(ex,"Erro ao processar evento de commit da mídia {MediaId}", payload.MediaFileId);
            }
        }

        public async Task HandleMediaDelete(string message)
        {
            var payload = JsonSerializer.Deserialize<MediaDeleteEvent>(message);
            if(payload == null)
            {
                this.logger.LogError("Erro ao deserializar payload de evento de deleção de midia.");
                return;
            }
            using var scope = this.scopeFactory.CreateScope();
            var mediaFileServices = scope.ServiceProvider.GetRequiredService<IMediaFileServices>();
            var media = await mediaFileServices.FindBy(m => m.Id == payload.MediaFileId && m.OwnerType == payload.OwnerType);
            if(media == null)
            {
                this.logger.LogWarning("Mídia não encontrada. MediaId: {MediaId}", payload.MediaFileId);
                return;
            }
            await mediaFileServices.DeleteImageAsync(media);
            this.logger.LogInformation("Midia deletada com sucesso");
        }
    }
}