using CommentService.Domain.Enums;
namespace CommentService.Application.Validations.Interfaces
{
    public interface ICommentValidationService
    {
        Task ValidateTargetExists(Guid targetId,CommentType type);
        Task ValidateUserExists(Guid userId);
        Task ValidatePostExists(Guid postId);
        Task ValidateToolExists(Guid toolId);
        Task ValidateCommentExists(Guid commentId);
        Task ValidateReply(Guid replyId);
    }
}