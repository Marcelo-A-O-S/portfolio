using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class PostToolsRepository : Generics<PostTool>, IPostToolsRepository
    {
        public PostToolsRepository(DBContext _context) : base(_context)
        {
        }
    }
}