using CommentService.Domain.Entities;
namespace CommentService.Domain.Interfaces
{
    public interface ILikeRepository : IGenerics<Like>
    {
        Task DeleteByCommentId(Guid commentId);
    }
}