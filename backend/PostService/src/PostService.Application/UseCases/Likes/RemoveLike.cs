using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Likes.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Likes
{
    public class RemoveLike : IRemoveLike
    {
        private readonly IPostServices postServices;
        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        public RemoveLike(
            IPostServices _postServices,
            ILikeServices _likeServices,
            ILikeCacheServices _likeCacheServices
        )
        {
            this.postServices = _postServices;
            this.likeServices = _likeServices;
            this.likeCacheServices = _likeCacheServices;
        }
        public async Task ExecuteAsync(Guid userId, LikeRequest likeRequest)
        {
            ValidateLikeRequest(likeRequest);
            await ValidatePostExists(likeRequest.PostId);
            var like = await GetLikeOrThrowAsync(userId, likeRequest.PostId);
            await this.likeServices.DeleteById(like.Id);
            await this.likeCacheServices.RemoveLikeCache($"like:post:{likeRequest.PostId}:user:{userId}");
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
        private async Task ValidatePostExists(Guid postId)
        {
            var exists = await this.postServices
                .FindBy(p => p.Id == postId);
            if (exists == null)
                throw new NotFoundException($"Projeto não encontrado");
        }
        private async Task<Like> GetLikeOrThrowAsync(Guid userId, Guid postId)
        {
            var cacheKey = $"post:{postId}:likes:user:{userId}";
            var like = await likeServices
                .FindBy(l => l.UserId == userId &&
                        l.PostId == postId);
            if (like == null)
            {
                await likeCacheServices.RemoveLikeCache(cacheKey);
                throw new ValidationException("Você não curtiu esse projeto!");
            }
            return like;
        }
    }
}