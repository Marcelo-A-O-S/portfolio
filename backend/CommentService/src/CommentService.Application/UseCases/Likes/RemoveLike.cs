using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Application.Validations.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Likes
{
    public class RemoveLike : IRemoveLike
    {

        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        private readonly ILikeValidationService likeValidationService;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public RemoveLike(
            ILikeServices _likeServices,
            ILikeCacheServices _likeCacheServices,
            ILikeValidationService _likeValidationService,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.likeServices = _likeServices;
            this.likeCacheServices = _likeCacheServices;
            this.likeValidationService = _likeValidationService;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, LikeRequest likeRequest)
        {
            await likeValidationService.ValidateUserExists(authenticatedUserId);
            await likeValidationService.ValidateTargetExists(likeRequest.TargetId, likeRequest.Type);
            var like = await GetLike(likeRequest.TargetId, authenticatedUserId);
            var type = like.Type.ToString();
            await this.likeServices.DeleteById(like.Id);
            await this.likeCacheServices.RemoveLikeCache($"like:{type}:{like.TargetId}:user:{like.UserId}");
            await this.rabbitMQProducer.Publish($"{type}Unliked", 
            new { 
                LikeId = like.Id,
                TargetId = like.TargetId, 
                Type = like.Type,
                UserId = like.UserId
                });
        }
        private async Task<Like> GetLike(Guid targetId, Guid authenticatedUserId)
        {
            var like = await this.likeServices.FindBy(
                    l => l.TargetId == targetId && l.UserId == authenticatedUserId);
            if(like == null)
                throw new NotFoundException("Você não curtiu esse comentário");
            return like;
        }
    }
}