using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Infrastructure.Jobs
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
                    var cleanupMedia = scope.ServiceProvider.GetRequiredService<IMediaFilesRepository>();
                    await cleanupMedia.DeleteExpiredPendingMediaAsync();
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