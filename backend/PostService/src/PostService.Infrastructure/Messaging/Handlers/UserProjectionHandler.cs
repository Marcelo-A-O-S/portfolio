using System.Text.Json;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostService.Application.Caching.Interfaces;
using PostService.Infrastructure.Messaging.Events;
using PostService.Application.Constants;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class UserProjectionHandler : IUserProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<UserProjectionHandler> logger;
        public UserProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<UserProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task RemoveUserCache(string message)
        {
            var payload = JsonSerializer.Deserialize<UserRemoveEvent>(message);
            if (payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IUserCacheServices>();
            await cache.RemoveUserCache(CacheKeys.UserExists(payload.UserId));
        }
    }
}