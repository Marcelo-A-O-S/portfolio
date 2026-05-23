using PostService.Application.UseCases.Likes.Interfaces;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Exceptions;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Likes
{
    public class AddLike : IAddLike
    {
        private readonly ILikeServices likeServices;
        private readonly IPostServices postServices;
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        public AddLike(
            ILikeServices _likeServices,
            IPostServices _postServices,
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient
        )
        {
            this.likeServices = _likeServices;
            this.postServices = _postServices;
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
        }
        public async Task ExecuteAsync(Guid userId, LikeRequest likeRequest)
        {
            ValidateLikeRequest(likeRequest);
            await ValidatePostExists(likeRequest.PostId);
            await ValidateUserExists(userId);
            var like = await ExistsAsync(userId, likeRequest.PostId);
            await this.likeServices.Save(like);
        }
        private static void ValidateLikeRequest(LikeRequest likeRequest)
        {
            var validationError = ValidationHelper.Validate(likeRequest);
            if(validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
        private async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:{userId}");
            if(userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if(!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:{userId}", userId);
            }
        }
        private async Task ValidatePostExists(Guid postId)
        {
            var exists = await this.postServices
                .FindBy(p => p.Id == postId);
            if(exists == null)
                throw new NotFoundException($"Projeto não encontrado");
        }
        private async Task<Like> ExistsAsync(Guid userId, Guid postId)
        {
            var exists = await this.likeServices
                .FindBy(l => l.UserId == userId && l.PostId == postId);
            if(exists != null)
                throw new ValidationException($"Você já curtiu esse projeto!");
            return new Like(userId, postId);
        }
    }
}