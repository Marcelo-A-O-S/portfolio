using CommentService.Domain.Entities;
namespace CommentService.Domain.Interfaces
{
    public interface ICommentRepository : IGenerics<Comment>
    {
        Task<List<Comment>> GetCommentsByPostId(Guid postId);
    }
}