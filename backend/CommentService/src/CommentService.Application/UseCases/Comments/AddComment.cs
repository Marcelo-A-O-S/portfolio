using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Domain.Entities;
using CommentService.Domain.Enums;
using Microsoft.Extensions.Logging;
namespace CommentService.Application.UseCases.Comments
{
    public class AddComment : IAddComment
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IUserServicesClient userServicesClient;
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IToolCacheServices toolCacheServices;
        private readonly IToolServicesClient toolServicesClient;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ILogger<AddComment> logger;
        public AddComment(
            IUserCacheServices _userCacheServices,
            IPostCacheServices _postCacheServices,
            IPostServicesClient _postServicesClient,
            IUserServicesClient _userServicesClient,
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IToolCacheServices _toolCacheServices,
            IToolServicesClient _toolServicesClient,
            IRabbitMQProducer _rabbitMQProducer,
            ILogger<AddComment> _logger
        )
        {
            this.userCacheServices = _userCacheServices;
            this.postCacheServices = _postCacheServices;
            this.postServicesClient = _postServicesClient;
            this.userServicesClient = _userServicesClient;
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.toolCacheServices = _toolCacheServices;
            this.toolServicesClient = _toolServicesClient;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.logger = _logger;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await ValidateUserExists(authenticatedUserId);
            await ValidateTargetExists(commentRequest.TargetId, commentRequest.Type);
            var comment = new Comment(authenticatedUserId, commentRequest.TargetId, commentRequest.Type, commentRequest.Content);
            await this.commentServices.Save(comment);
            await this.commentCacheServices.AddCommentCache($"comment:exists:{comment.Id}", comment.Id);
            await this.rabbitMQProducer.Publish("CommentCreated", new { CommentId = comment.Id});
        }
        private static void ValidateRequest(CommentRequest commentRequest)
        {
            var validationError = ValidationHelper.Validate(commentRequest);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ",validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:exists:{userId}");
            if(userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:exists:{userId}", userId);
            }
        }
        private async Task ValidateTargetExists(Guid targetId, CommentType type)
        {
            switch (type)
            {
                case CommentType.Post:
                    await ValidatePostExists(targetId);
                    break;
                case CommentType.Tool:
                    await ValidateToolExists(targetId);
                    break;
                default:
                    throw new ValidationException("Tipo de comentário inválido.");
            }
        }
        private async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache($"post:exists:{postId}");
            if(postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache($"post:exists:{postId}", postId);
            }
        }
        private async Task ValidateToolExists(Guid toolId)
        {
            var toolCache = await this.toolCacheServices.GetToolCache($"tool:exists:{toolId}");
            if(toolCache == null)
            {
                var exists = await this.toolServicesClient.ToolExistsAsync(toolId);
                if(!exists)
                    throw new NotFoundException("Ferramenta não encontrada");
                await this.toolCacheServices.AddToolCache($"tool:exists:{toolId}", toolId);
            }
        }
        
    }
}