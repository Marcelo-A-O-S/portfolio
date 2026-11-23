using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace MediaService.Infrastructure.Jobs
{
    public class CleanupMediaJob : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CleanupMediaJob> logger;
        public CleanupMediaJob(
            IServiceProvider _serviceProvider,
            ILogger<CleanupMediaJob> _logger
        )
        {
            this.serviceProvider = _serviceProvider;
            this.logger = _logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this.logger.LogInformation("Iniciando limpeza de imagens não utilizadas...");
                    using var scope = this.serviceProvider.CreateScope();
                    var mediaFileServices = scope.ServiceProvider.GetRequiredService<IMediaServices>();
                    var mediaOwnerServices = scope.ServiceProvider.GetRequiredService<IMediaOwnerServices>();
                    var mediaDeletionServices = scope.ServiceProvider.GetRequiredService<IMediaDeletionServices>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var medias = await mediaFileServices.ListExpiredUncommittedMediaAsync();
                    this.logger.LogInformation("Foram encontradas {Count} mídias expiradas.", medias.Count);
                    foreach (var media in medias)
                    {
                        await unitOfWork.BeginAsync();
                        try
                        {
                            foreach (var mediaOwner in media.MediaOwners)
                            {
                                media.RemoveOwner(mediaOwner);
                                await mediaOwnerServices.Delete(mediaOwner);
                            }
                            if(media.MediaOwners.Count == 0)
                            {
                                var mediaDeletion = new MediaDeletion(
                                    media.Path
                                );
                                await mediaFileServices.Delete(media);
                                await mediaDeletionServices.Save(mediaDeletion);
                            }
                            await unitOfWork.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogError(ex, "Erro ao remover mídia {MediaId}", media.Id);
                            await unitOfWork.RollbackAsync();
                            throw;
                        }
                    }
                    this.logger.LogInformation("Limpeza de imagens não utilizadas realizada com sucesso!");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Erro ao limpar imagens.");
                }
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}