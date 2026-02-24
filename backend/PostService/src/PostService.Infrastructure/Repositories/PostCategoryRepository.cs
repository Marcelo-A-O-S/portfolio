using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class PostCategoryRepository : Generics<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(DBContext _context) : base(_context)
        {
        }
    }
}