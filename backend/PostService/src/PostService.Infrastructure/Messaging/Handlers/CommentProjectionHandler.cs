using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostService.Application.Interfaces;
using System.Text.Json;
using PostService.Infrastructure.Messaging.Events;
using PostService.Domain.Enums;
using PostService.Domain.Entities;
namespace PostService.Infrastructure.Messaging.Handlers
{
    public class CommentProjectionHandler : ICommentProjectionHandler
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<CommentProjectionHandler> logger;
        public CommentProjectionHandler(
            IServiceScopeFactory _scopeFactory,
            ILogger<CommentProjectionHandler> _logger
        )
        {
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task HandleCommentAdded(string message)
        {
            var payload = JsonSerializer.Deserialize<CommentEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case CommentType.Post:
                    await AddPostCommentCount(payload);
                    break;
                case CommentType.Tool:
                    await AddToolCommentCount(payload);
                    break;
            }
        }
        public async Task HandleCommentRemoved(string message)
        {
            var payload = JsonSerializer.Deserialize<CommentEvent>(message);
            if (payload == null)
                return;
            switch (payload.Type)
            {
                case CommentType.Post:
                    await RemovePostCommentCount (payload);
                    break;
                case CommentType.Tool:
                    await RemoveToolCommentCount(payload);
                    break;
            }
        }
        private async Task AddPostCommentCount(CommentEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            await postServices.IncrementCommentCount(post.Id);
        }
        private async Task RemovePostCommentCount(CommentEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var postServices = scope.ServiceProvider.GetRequiredService<IPostServices>();
            var post = await postServices.GetById(payload.TargetId);
            if (post == null)
            {
                this.logger.LogWarning("Evento recebido para post inexistente. PostId: {PostId}", payload.TargetId);
                return;
            }
            await postServices.DecrementCommentCount(post.Id);
        }
        private async Task AddToolCommentCount(CommentEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var toolsServices = scope.ServiceProvider.GetRequiredService<IToolsServices>();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            await toolsServices.IncrementCommentCount(tool.Id);
        }
        private async Task RemoveToolCommentCount(CommentEvent payload)
        {
            using var scope = this.scopeFactory.CreateScope();
            var toolsServices = scope.ServiceProvider.GetRequiredService<IToolsServices>();
            var tool = await toolsServices.GetById(payload.TargetId);
            if(tool == null)
            {
                logger.LogWarning("Evento recebido para ferramenta inexistente. ToolId: {ToolId}", payload.TargetId);
                return;
            }
            await toolsServices.DecrementCommentCount(tool.Id);
        }
    }
}