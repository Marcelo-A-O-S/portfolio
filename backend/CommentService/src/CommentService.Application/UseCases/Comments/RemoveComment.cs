using CommentService.Application.Caching.Comment;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Enums;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveComment : IRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        private readonly IUserProjectionServices userProjectionServices;
        private readonly ILikeServices likeServices;
        private readonly IUnitOfWork unitOfWork;
        public RemoveComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService,
            IUserProjectionServices _userProjectionServices,
            ILikeServices _likeServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
            this.userProjectionServices = _userProjectionServices;
            this.likeServices = _likeServices;
            this.unitOfWork = _unitOfWork;
        }

        public async Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            if(!Enum.TryParse<UserRole>(role,true, out var userRole))
                throw new ValidationException("Usuário inválido.");
            var comment = await GetComment(commentId);
            if(userRole == UserRole.Client && comment.UserProjection.UserId != authenticatedUserId)
            {
                throw new ValidationException("Você não pode remover este comentário."); 
            }
            await this.unitOfWork.BeginAsync();
            try
            {
                switch (userRole)
                {
                    case UserRole.Administrador:
                        comment.DeleteByAdministrador();
                        break;
                    case UserRole.Moderator:
                        comment.DeleteByModerator();
                        break;
                    case UserRole.Client:
                        comment.DeleteByUser();
                        break;
                    default:
                        throw new ValidationException("Usuário inválido");
                }
                await this.commentServices.Update(comment);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            var type = comment.Type.ToString();
            await this.commentCacheServices.AddCommentCache($"comment:{type}:exists:{comment.Id}", comment.Id);
            await this.rabbitMQProducer.Publish($"{type}CommentDeleted",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = comment.UserProjection.UserId
            });
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetFullDataById(commentId);
            if(comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        } 
    }
}