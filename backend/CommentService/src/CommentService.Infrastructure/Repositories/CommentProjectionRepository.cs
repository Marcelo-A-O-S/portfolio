using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
namespace CommentService.Infrastructure.Repositories
{
    public class CommentProjectionRepository : Generics<CommentProjection>, ICommentProjectionRepository
    {
        public CommentProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}