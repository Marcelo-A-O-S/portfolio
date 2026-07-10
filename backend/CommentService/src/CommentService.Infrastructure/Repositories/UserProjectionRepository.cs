using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;

namespace CommentService.Infrastructure.Repositories
{
    public class UserProjectionRepository : Generics<UserProjection>, IUserProjectionRepository
    {
        public UserProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}