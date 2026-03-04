using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;

namespace CommentService.Infrastructure.Repositories
{
    public class CommentRepository : Generics<Comment>, ICommentRepository
    {
        public CommentRepository(DBContext _context) : base(_context)
        {
        }
    }
}