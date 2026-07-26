using CommentService.Domain.Entities;
using CommentService.Domain.Enums;
using CommentService.Domain.Queries;

namespace CommentService.Application.Interfaces
{
    public interface ICommentServices : IServices<Comment>
    {
        Task<List<Comment>> GetCommentsByTargetId(Guid targetId);
        Task<List<Comment>> GetCommentsByTargeIdsPage(List<Guid> targetIds);
        Task<Dictionary<Guid, int>> GetQuantityCommentsByTargeIdsPage(List<Guid> targetIds);
        Task<PaginatedResult<CommentView>> GetCommentsPaginationByTargetAndType(Guid? authenticatedUserId, Guid targetId, CommentType type,int page, int itemsPage = 10);
        Task<Comment> GetFullDataById(Guid id);
    }
}