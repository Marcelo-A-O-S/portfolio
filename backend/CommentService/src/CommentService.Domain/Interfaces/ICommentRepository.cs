using CommentService.Domain.Entities;
using System.Collections.Generic;
namespace CommentService.Domain.Interfaces
{
    public interface ICommentRepository : IGenerics<Comment>
    {
        Task<List<Comment>> GetCommentsByPostId(Guid postId);
        Task<List<Comment>> GetCommentsByPostIdsPage(List<Guid> postIds);
        Task<Dictionary<Guid, int>> GetQuantityCommentsByPostIdsPage(List<Guid> postIds);
    }
}