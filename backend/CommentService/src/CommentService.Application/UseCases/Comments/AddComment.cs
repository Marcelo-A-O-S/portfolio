using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;

namespace CommentService.Application.UseCases.Comments
{
    public class AddComment : IAddComment
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IPostCacheServices postCacheServices;
        public AddComment(
            IUserCacheServices _userCacheServices,
            IPostCacheServices _postCacheServices
        )
        {
            this.userCacheServices = _userCacheServices;
            this.postCacheServices = _postCacheServices;
        }
        public Task ExecuteAsync(CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            throw new NotImplementedException();
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
                
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            
        }
    }
}