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
            var mediaFileServices = scope.ServiceProvider.GetRequiredService<IMediaServices>();
            var mediaOwnnerServices = scope.ServiceProvider.GetRequiredService<IMediaOwnerServices>();
            var mediaValidationServices = scope.ServiceProvider.GetRequiredService<IMediaValidationServices>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var media = await mediaFileServices
                .GetMediaBy(m => 
                m.Id == payload.MediaId && 
                m.MediaOwners.Any(mo => 
                mo.OwnerId == payload.OwnerId && 
                mo.OwnerType == payload.OwnerType));
            if(media == null)
            {
                this.logger.LogWarning("Midia não encontrada.");
                return;
            }
            await mediaValidationServices.ValidateOwnerExists(payload.OwnerId, payload.OwnerType);
            media.Commit();
            await unitOfWork.BeginAsync();
            try
            {
                await mediaFileServices.Update(media);
                await unitOfWork.CommitAsync();
                this.logger.LogInformation("Midia commitada com sucesso");
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex,"Erro ao processar evento de commit da mídia {MediaId}", payload.MediaId);
                await unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task HandleMediaDelete(string message)
        {
            var payload = JsonSerializer.Deserialize<MediaDeleteEvent>(message);
            if(payload == null)
            {
                this.logger.LogWarning("Erro ao deserializar payload de evento de deleção de midia.");
                return;
            }
            using var scope = this.scopeFactory.CreateScope();
            var mediaFileServices = scope.ServiceProvider.GetRequiredService<IMediaServices>();
            var mediaOwnerServices = scope.ServiceProvider.GetRequiredService<IMediaOwnerServices>();
            var mediaStorage = scope.ServiceProvider.GetRequiredService<IMediaStorage>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var media = await mediaFileServices
                .GetMediaBy(m => 
                m.Id == payload.MediaId && 
                m.MediaOwners.Any(mo => 
                mo.MediaId == payload.MediaId && 
                mo.OwnerId == payload.OwnerId &&
                mo.OwnerType == payload.OwnerType));
            if(media == null)
            {
                this.logger.LogWarning("Mídia não encontrada. MediaId: {MediaId}", payload.MediaId);
                return;
            }
            await unitOfWork.BeginAsync();
            var removeOwner = media.MediaOwners
                .Where(m => m.MediaId == payload.MediaId 
                && m.OwnerType == payload.OwnerType)
                .FirstOrDefault();
            var isMediaDeleted = false;
            try
            {
                if (removeOwner != null)
                {
                    media.RemoveOwner(removeOwner);
                    await mediaOwnerServices.Delete(removeOwner);
                }
                if(media.MediaOwners.Count == 0)
                {
                    await mediaFileServices.Delete(media);
                    isMediaDeleted = true;
                }
                await unitOfWork.CommitAsync();
                if (isMediaDeleted)
                {
                    await mediaStorage.DeleteAsync(media.Path);
                }
                this.logger.LogInformation("Midia deletada com sucesso");
            }   
            catch(Exception ex)
            {
                this.logger.LogWarning(ex,"Erro ao processar a remoção da imagem do banco");
                await unitOfWork.RollbackAsync();
                throw;
            }
            
        }
    }
}