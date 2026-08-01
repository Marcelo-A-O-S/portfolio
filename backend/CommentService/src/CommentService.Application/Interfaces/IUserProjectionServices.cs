using CommentService.Domain.Entities;

namespace CommentService.Application.Interfaces
{
    public interface IUserProjectionServices : IServices<UserProjection>
    {
        Task<UserProjection> GetByUserId(Guid userId);
    }
}