using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Infrastructure.Repositories
{
    public class UserProjectionRepository : Generics<UserProjection>, IUserProjectionRepository
    {
        private readonly DBContext context;
        public UserProjectionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<UserProjection> GetByUserId(Guid userId)
        {
            return await this.context.UserProjections.Where(up => up.UserId == userId).FirstOrDefaultAsync();
        }
    }
}