using CommentService.Application.Caching.Like;
using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using System.Text.Json;
namespace CommentService.Application.UseCases.Likes
{
    public class RemoveLike : IRemoveLike
    {
        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        private readonly ILikeValidationService likeValidationService;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public RemoveLike(
            ILikeServices _likeServices,
            ILikeCacheServices _likeCacheServices,
            ILikeValidationService _likeValidationService,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.likeServices = _likeServices;
            this.likeCacheServices = _likeCacheServices;
            this.likeValidationService = _likeValidationService;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, LikeRequest likeRequest)
        {
            await likeValidationService.ValidateUserExists(authenticatedUserId);
            await likeValidationService.ValidateTargetExists(likeRequest.TargetId, likeRequest.Type);
            var like = await GetLike(likeRequest.TargetId, authenticatedUserId);
            var type = like.Type.ToString();
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.likeServices.DeleteById(like.Id);
                var outboxMessage = new OutboxMessage(
                $"{type}Unliked",
                JsonSerializer.Serialize(new
                {
                    LikeId = like.Id,
                    TargetId = like.TargetId,
                    Type = like.Type,
                     UserId = like.UserId
                }));
                await this.outboxMessageServices.Save(outboxMessage);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await this.likeCacheServices.RemoveLikeCache($"like:{type}:{like.TargetId}:user:{like.UserId}");
        }
        private async Task<Like> GetLike(Guid targetId, Guid authenticatedUserId)
        {
            var like = await this.likeServices.FindBy(
                    l => l.TargetId == targetId && l.UserId == authenticatedUserId);
            if (like == null)
                throw new NotFoundException("Você não curtiu esse comentário");
            return like;
        }
    }
}