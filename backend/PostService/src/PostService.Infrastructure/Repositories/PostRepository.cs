using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : Generics<Post>, IPostRepository
    {
        public PostRepository(DBContext _context) : base(_context)
        {
        }
    }
}