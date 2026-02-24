using AuthService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace AuthService.Infrastructure.Jobs
{
    public class RefreshTokenCleanupJob : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<RefreshTokenCleanupJob> logger;
        public RefreshTokenCleanupJob(IServiceProvider _serviceProvider, ILogger<RefreshTokenCleanupJob> _logger)
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
                    var cleanup = scope.ServiceProvider.GetRequiredService<IRefreshTokenServices>();
                    await cleanup.CleanupExpiredTokensAsync();
                    this.logger.LogInformation("Limpeza de tokens expirados realizada.");
                }catch(Exception ex)
                {
                    this.logger.LogError(ex, "Error ao limpar tokens expirados.");
                }
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}