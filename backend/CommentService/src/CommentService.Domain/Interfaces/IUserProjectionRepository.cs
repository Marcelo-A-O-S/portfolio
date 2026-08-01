using CommentService.Domain.Entities;
namespace CommentService.Domain.Interfaces
{
    public interface IUserProjectionRepository : IGenerics<UserProjection>
    {
        Task<UserProjection> GetByUserId(Guid userId);
    }
}