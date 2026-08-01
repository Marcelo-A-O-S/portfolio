using CommentService.Application.Caching.Comment;
using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class AddReply : IAddReply
    {
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentServices commentServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        private readonly IUserServicesClient userServicesClient;
        private readonly IUserProjectionServices userProjectionServices;
        public AddReply(
            ICommentCacheServices _commentCacheServices,
            ICommentServices _commentServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService,
            IUserServicesClient _userServicesClient,
            IUserProjectionServices _userProjectionServices
        )
        {
            this.commentCacheServices = _commentCacheServices;
            this.commentServices = _commentServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
            this.userServicesClient = _userServicesClient;
            this.userProjectionServices = _userProjectionServices;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, string providerId, Guid commentId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await this.commentValidationService.ValidateProviderExists(authenticatedUserId, providerId);
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            await this.commentValidationService.ValidateTargetExists(commentRequest.TargetId, commentRequest.Type);
            var comment = await GetComment(commentId);
            if (comment.TargetId != commentRequest.TargetId)
                throw new ValidationException("Comentário não pertence ao post informado.");
            var reply = new Comment(commentRequest.TargetId, commentRequest.Type, commentRequest.Content, comment.Id);
            var user = await this.userServicesClient.GetUserAsync(authenticatedUserId, providerId);
            var userProjection = await this.userProjectionServices
                .FindBy(up => up.UserId == authenticatedUserId && up.ProviderId == providerId);
            if(userProjection == null)
            {
                userProjection = new UserProjection(authenticatedUserId, user.Name, user.ProfileUrl, user.ProviderId, user.Provider);
                await this.userProjectionServices.Save(userProjection);
            }
            reply.SetUserProjectionId(userProjection.Id);
            await this.commentServices.Save(reply);
            var type = reply.Type.ToString();
            await this.commentCacheServices.AddCommentCache($"comment:{type}:exists:{reply.Id}", reply.Id);
            await this.rabbitMQProducer.Publish($"{type}ReplyCreated",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = reply.UserProjection.UserId
            });
        }
        private static void ValidateRequest(CommentRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetById(commentId);
            if (comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        }
    }
}