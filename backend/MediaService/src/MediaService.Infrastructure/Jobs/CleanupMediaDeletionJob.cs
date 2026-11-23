using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MediaService.Application.Interfaces;

namespace MediaService.Infrastructure.Jobs 
{
    public class CleanupMediaDeletionJob : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CleanupMediaDeletionJob> logger;
        public CleanupMediaDeletionJob(
            IServiceProvider _serviceProvider,
            ILogger<CleanupMediaDeletionJob> _logger
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
                    using var scope = this.serviceProvider.CreateScope();
                    var mediaDeletionServices = scope.ServiceProvider.GetRequiredService<IMediaDeletionServices>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var medias = await mediaDeletionServices.GetMediasDeletionProcessed();
                    foreach (var item in medias)
                    {
                        await unitOfWork.BeginAsync();
                        try
                        {
                            await mediaDeletionServices.Delete(item);
                            await unitOfWork.CommitAsync();
                        }catch(Exception ex)
                        {
                            await unitOfWork.RollbackAsync();
                            throw;
                        }
                    }
                }catch(Exception ex)
                {
                    this.logger.LogError(ex, "Erro ao limpar objetos de imagem.");
                }
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}