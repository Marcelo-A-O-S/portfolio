using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;

namespace CommentService.Infrastructure.Repositories
{
    public class AnswerRepository : Generics<Answer>, IAnswerRepository
    {
        public AnswerRepository(DBContext _context) : base(_context)
        {
        }
    }
}