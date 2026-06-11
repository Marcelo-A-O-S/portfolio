using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Validations.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Likes
{
    public class AddLike : IAddLike
    {
        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        private readonly ILikeValidationService likeValidationService;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public AddLike(
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
            await this.likeValidationService.ValidateUserExists(authenticatedUserId);
            await this.likeValidationService.ValidateTargetExists(likeRequest.TargetId, likeRequest.Type);
            try
            {
                var like = new Like(likeRequest.TargetId, likeRequest.Type, authenticatedUserId);
                await this.likeServices.Save(like);
                var type = like.Type.ToString();
                await this.likeCacheServices.AddLikeCache($"like:{like.Type.ToString()}:{like.TargetId}:user:{like.UserId}", like.Id);
                await this.rabbitMQProducer.Publish($"{type}Liked", 
                new { 
                    LikeId = like.Id,
                    TargetId = like.TargetId, 
                    Type = like.Type,
                    UserId = like.UserId
                });
            }
            catch (DuplicateException)
            {
                throw new ValidationException("Você já curtiu esse comentário!");
            }
        }
    }
}