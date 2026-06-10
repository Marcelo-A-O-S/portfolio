using CommentService.Domain.Entities;
using System.Collections.Generic;
namespace CommentService.Domain.Interfaces
{
    public interface ICommentRepository : IGenerics<Comment>
    {
        Task<List<Comment>> GetCommentsByTargetId(Guid targetId);
        Task<List<Comment>> GetCommentsByTargeIdsPage(List<Guid> targetIds);
        Task<Dictionary<Guid, int>> GetQuantityCommentsByTargeIdsPage(List<Guid> targetIds);
    }
}