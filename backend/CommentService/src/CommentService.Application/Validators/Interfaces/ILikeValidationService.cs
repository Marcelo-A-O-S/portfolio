using CommentService.Domain.Enums;
namespace CommentService.Application.Validators.Interfaces
{
    public interface ILikeValidationService
    {
        Task ValidateUserExists(Guid userId);
        Task ValidateCommentExists(Guid commentId);
        Task ValidateTargetExists(Guid targetId, LikeType type);
        Task ValidateToolExists(Guid toolId);
        Task ValidatePostExists(Guid postId);
    }
}