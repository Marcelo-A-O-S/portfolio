using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class CategoryContentRepository : Generics<CategoryContent>, ICategoryContentRepository
    {
        public CategoryContentRepository(DBContext _context) : base(_context)
        {
        }
    }
}