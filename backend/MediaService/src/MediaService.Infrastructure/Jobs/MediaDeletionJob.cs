using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaService.Infrastructure.Jobs
{
    public class MediaDeletionJob : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<MediaDeletionJob> logger;
        public MediaDeletionJob(
            IServiceScopeFactory _scopeFactory,
            ILogger<MediaDeletionJob> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = this.scopeFactory.CreateScope();
                    var mediaDeletionServices = scope.ServiceProvider.GetRequiredService<IMediaDeletionServices>();
                    var mediaStorage = scope.ServiceProvider.GetRequiredService<IMediaStorage>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var medias = await mediaDeletionServices.GetMediaPending();
                    foreach (var media in medias)
                    {
                        try
                        {
                            await mediaStorage.DeleteAsync(media.Path);
                            await ChangeMediaDeletionStatus(
                                mediaDeletionServices, 
                                unitOfWork, 
                                media, 
                                mediaDeletion => mediaDeletion.Processed());
                        }
                        catch (Exception ex)
                        {
                            await ChangeMediaDeletionStatus(
                                mediaDeletionServices, 
                                unitOfWork, 
                                media, 
                                mediaDeletion => mediaDeletion.Failure());
                        }
                    }
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        exception,
                        "Erro ao processar a Media Deletion Job.");
                }
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
        private async Task ChangeMediaDeletionStatus(
            IMediaDeletionServices mediaDeletionServices,
            IUnitOfWork unitOfWork,
            MediaDeletion mediaDeletion,
            Action<MediaDeletion> changeStatus
        )
        {
            await unitOfWork.BeginAsync();
            try
            {
                changeStatus(mediaDeletion);
                await mediaDeletionServices.Update(mediaDeletion);
                await unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                await unitOfWork.ReloadAsync(mediaDeletion);
                throw;
            }
        }
    }
}