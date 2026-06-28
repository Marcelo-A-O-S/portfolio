using CommentService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CommentService.Infrastructure.Messaging.Events;
using CommentService.Application.Interfaces;

namespace CommentService.Infrastructure.Messaging.Handlers
{
    public class UserEventHandler : IUserEventHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<UserEventHandler> logger;
        public UserEventHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<UserEventHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task RemoveCache(string message)
        {
            var payload = JsonSerializer.Deserialize<UserRemoveEvent>(message);
            if(payload == null)
                return;
            using var scope = this.scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IUserCacheServices>();
            await cache.RemoveUserCache($"user:exists:{payload.UserId}");
        }
    }
}