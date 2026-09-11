using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class AuthorRepository : Generics<Author>, IAuthorRepository
    {
        private readonly DBContext context;
        public AuthorRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<Author> GetByUserId(Guid userId)
        {
            return await this.context.Authors
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }
    }
}