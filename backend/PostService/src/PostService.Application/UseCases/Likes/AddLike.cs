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
        public AddLike(
            ILikeServices _likeServices,
            IPostServices _postServices
        )
        {
            this.likeServices = _likeServices;
            this.postServices = _postServices;
        }
        public async Task ExecuteAsync(Guid userId, LikeRequest likeRequest)
        {
            ValidateLikeRequest(likeRequest);
            await ValidatePostExists(likeRequest.PostId);
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