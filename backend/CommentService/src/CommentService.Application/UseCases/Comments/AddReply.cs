using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class AddReply : IAddReply
    {
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentServices commentServices;
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostServicesClient postServicesClient;
        public AddReply(
            ICommentCacheServices _commentCacheServices,
            ICommentServices _commentServices,
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient,
            IPostCacheServices _postCacheServices,
            IPostServicesClient _postServicesClient
        )
        {
            this.commentCacheServices = _commentCacheServices;
            this.commentServices = _commentServices;
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
            this.postCacheServices = _postCacheServices;
            this.postServicesClient = _postServicesClient;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await ValidatePostExists(commentRequest.PostId);
            await ValidateUserExists(authenticatedUserId);
            await ValidateCommentExists(commentId);
            var comment = await GetComment(commentId);
            if(comment.PostId != commentRequest.PostId)
                throw new ValidationException("Comentário não pertence ao post informado.");
            var reply = new Comment(authenticatedUserId, commentRequest.PostId, commentRequest.Content, comment.Id);
            await this.commentServices.Save(reply);
            await this.commentCacheServices.AddCommentCache($"comment:reply:exists:{reply.Id}", reply.Id);
        }
        private static void ValidateRequest(CommentRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ",validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache($"post:exists:{postId}");
            if (postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.postCacheServices.AddPostCache($"post:exists:{postId}", postId);
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:exists:{userId}");
            if (userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:exists:{userId}", userId);
            }
        }
        private async Task ValidateCommentExists(Guid commentId)
        {
            var commentCache = await this.commentCacheServices.GetCommentCache($"comment:exists:{commentId}");
            if (commentCache == null)
            {
                var exists = await this.commentServices.Exists(commentId);
                if (!exists)
                    throw new NotFoundException("Commentário não encontrado");
                await this.commentCacheServices.AddCommentCache($"comment:exists:{commentId}", commentId);
            }
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetById(commentId);
            if(comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        }
    }
}