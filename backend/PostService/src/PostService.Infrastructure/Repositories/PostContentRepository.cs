using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class PostContentRepository : Generics<PostContent>, IPostContentRepository
    {
        public PostContentRepository(DBContext _context) : base(_context)
        {
        }
    }
}