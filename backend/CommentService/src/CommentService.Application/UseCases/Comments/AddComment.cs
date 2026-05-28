using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Domain.Entities;
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
        public AddComment(
            IUserCacheServices _userCacheServices,
            IPostCacheServices _postCacheServices,
            IPostServicesClient _postServicesClient,
            IUserServicesClient _userServicesClient,
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices
        )
        {
            this.userCacheServices = _userCacheServices;
            this.postCacheServices = _postCacheServices;
            this.postServicesClient = _postServicesClient;
            this.userServicesClient = _userServicesClient;
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
        }
        public async Task ExecuteAsync(CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await ValidateUserExists(commentRequest.UserId);
            await ValidatePostExists(commentRequest.PostId);
            var comment = new Comment(commentRequest.UserId, commentRequest.PostId, commentRequest.Content);
            await this.commentServices.Save(comment);
            await this.commentCacheServices.AddCommentCache($"comment:{comment.Id}", comment.Id);
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
        private async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache($"post:{postId}");
            if(postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.postCacheServices.AddPostCache($"post:{postId}", postId);
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:{userId}");
            if(userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:{userId}", userId);
            }
        }
    }
}