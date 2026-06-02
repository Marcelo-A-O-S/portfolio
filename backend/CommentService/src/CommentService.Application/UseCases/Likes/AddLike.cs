using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;

namespace CommentService.Application.UseCases.Likes
{
    public class AddLike : IAddLike
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentServices commentServices;
        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        public AddLike(
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient,
            ICommentCacheServices _commentCacheServices,
            ICommentServices _commentServices,
            ILikeServices _likeServices,
            ILikeCacheServices _likeCacheServices
        )
        {
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
            this.commentCacheServices = _commentCacheServices;
            this.commentServices = _commentServices;
            this.likeServices = _likeServices;
            this.likeCacheServices = _likeCacheServices;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId)
        {
            await ValidateUserExists(authenticatedUserId);
            await ValidateCommentExists(commentId);
            try
            {
                var like = new Like(commentId, authenticatedUserId);
                await this.likeServices.Save(like);
                await this.likeCacheServices.AddLikeCache($"like:comment:{like.CommentId}:user:{like.UserId}", like.Id);
            }
            catch (DuplicateException)
            {
                throw new ValidationException("Você já curtiu esse comentário!");
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
    }
}