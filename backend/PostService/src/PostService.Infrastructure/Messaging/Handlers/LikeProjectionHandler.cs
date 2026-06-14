using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using PostService.Domain.Enums;
using PostService.Infrastructure.Messaging.Events;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PostService.Domain.Entities;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class LikeProjectionHandler : ILikeProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<LikeProjectionHandler> logger;
        public LikeProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<LikeProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleLikeAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<LikedEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case LikeType.Post:
                    await AddPostLikeCount(payload);
                    break;
                case LikeType.Tool:
                    await AddToolLikeCount(payload);
                    break;
            }
        }
        public async Task HandleLikeRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<LikedEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case LikeType.Post:
                    await RemovePostLikeCount(payload);
                    break;
                case LikeType.Tool:
                    await RemoveToolLikeCount(payload);
                    break;
            }
        }
        private async Task AddPostLikeCount(LikedEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            var likeProjectionServices = scope.ServiceProvider.GetRequiredService<ILikeProjectionServices>();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            var likeProjection = new LikeProjection(payload.TargetId, payload.UserId);
            await likeProjectionServices.Save(likeProjection);
            await postServices.IncrementLikeCount(post.Id);
        }
        private async Task RemovePostLikeCount(LikedEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            var likeProjectionServices = scope.ServiceProvider.GetRequiredService<ILikeProjectionServices>();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            var likeProjection = await likeProjectionServices.FindBy(lp => lp.TargetId == payload.TargetId && lp.UserId == payload.UserId);
            if(likeProjection == null)
            {
                this.logger.LogWarning("Evento recebido para Like inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            await likeProjectionServices.Delete(likeProjection);
            await postServices.DecrementLikeCount(post.Id);
        }
        private async Task AddToolLikeCount(LikedEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var likeProjectionServices = scope.ServiceProvider.GetRequiredService<ILikeProjectionServices>();
            var toolsServices = scope.ServiceProvider.GetRequiredService<IToolsServices>();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            var likeProjection = new LikeProjection(payload.TargetId, payload.UserId);
            await likeProjectionServices.Save(likeProjection);
            await toolsServices.IncrementLikeCount(tool.Id);
        }
        private async Task RemoveToolLikeCount(LikedEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var likeProjectionServices = scope.ServiceProvider.GetRequiredService<ILikeProjectionServices>();
            var toolsServices = scope.ServiceProvider.GetRequiredService<IToolsServices>();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            var likeProjection = await likeProjectionServices.FindBy(lp => lp.TargetId == payload.TargetId && lp.UserId == payload.UserId);
            if(likeProjection == null)
            {
                this.logger.LogWarning("Evento recebido para Like inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            await likeProjectionServices.Delete(likeProjection);
            await toolsServices.DecrementLikeCount(tool.Id);
        }
    }
}