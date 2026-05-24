using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Likes.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Likes
{
    public class AddLike : IAddLike
    {
        private readonly ILikeServices likeServices;
        private readonly IPostServices postServices;
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        private readonly ILikeCacheServices likeCacheServices;
        public AddLike(
            ILikeServices _likeServices,
            IPostServices _postServices,
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient,
            ILikeCacheServices _likeCacheServices
        )
        {
            this.likeServices = _likeServices;
            this.postServices = _postServices;
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
            this.likeCacheServices = _likeCacheServices;
        }
        public async Task ExecuteAsync(Guid userId, LikeRequest likeRequest)
        {
            ValidateLikeRequest(likeRequest);
            await ValidatePostExists(likeRequest.PostId);
            await ValidateUserExists(userId);
            await ValidateLikeExists(userId, likeRequest.PostId);
            try
            {
                var like = new Like(userId, likeRequest.PostId);
                await this.likeServices.Save(like);
                await this.likeCacheServices.AddLikeCache($"post:{like.PostId}:likes:user:{like.UserId}", true);
            }
            catch (DuplicateException)
            {
                throw new ValidationException(
                        "Você já curtiu esse projeto!"
                    );
            }
        }
        private static void ValidateLikeRequest(LikeRequest likeRequest)
        {
            var validationError = ValidationHelper.Validate(likeRequest);
            if (validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:{userId}");
            if (userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:{userId}", userId);
            }
        }
        private async Task ValidatePostExists(Guid postId)
        {
            var exists = await this.postServices
                .FindBy(p => p.Id == postId);
            if (exists == null)
                throw new NotFoundException($"Projeto não encontrado");
        }
        private async Task ValidateLikeExists(Guid userId, Guid postId)
        {
            var existsCache = await this.likeCacheServices.GetLikeCache($"post:{postId}:likes:user:{userId}");
            if (existsCache != null)
                throw new ValidationException($"Você já curtiu esse projeto!");
            var exists = await this.likeServices
                .FindBy(l => l.UserId == userId && l.PostId == postId);
            if (exists != null)
            {
                await this.likeCacheServices.AddLikeCache($"post:{postId}:likes:user:{userId}", true);
                throw new ValidationException($"Você já curtiu esse projeto!");
            }
        }
    }
}