using CommentService.Domain.Entities;

namespace CommentService.Application.Interfaces
{
    public interface ICommentServices : IServices<Comment>
    {
        Task<List<Comment>> GetCommentsByPostId(Guid postId);
    }
}