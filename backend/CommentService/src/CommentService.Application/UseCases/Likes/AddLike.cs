using System.Text.Json;
using CommentService.Application.Caching.Like;
using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Likes
{
    public class AddLike : IAddLike
    {
        private readonly ILikeServices likeServices;
        private readonly ILikeCacheServices likeCacheServices;
        private readonly ILikeValidationService likeValidationService;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public AddLike(
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
            await this.likeValidationService.ValidateUserExists(authenticatedUserId);
            await this.likeValidationService.ValidateTargetExists(likeRequest.TargetId, likeRequest.Type);
            await this.unitOfWork.BeginAsync();
            try
            {
                
                var like = new Like(likeRequest.TargetId, likeRequest.Type, authenticatedUserId);
                var type = like.Type.ToString();
                try
                {
                    await this.likeServices.Save(like);
                    var outboxMessage = new OutboxMessage(
                        $"{type}Liked",
                        JsonSerializer.Serialize(new { 
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
                await this.likeCacheServices.AddLikeCache($"like:{like.Type.ToString()}:{like.TargetId}:user:{like.UserId}", like.Id);
            }
            catch (DuplicateException)
            {
                throw new ValidationException("Você já curtiu esse comentário!");
            }
        }
    }
}