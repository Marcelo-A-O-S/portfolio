using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class AuthorRepository : Generics<Author>, IAuthorRepository
    {
        public AuthorRepository(DBContext _context) : base(_context)
        {
        }
    }
}