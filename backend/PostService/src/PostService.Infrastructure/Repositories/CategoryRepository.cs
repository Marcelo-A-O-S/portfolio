using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class CategoryRepository : Generics<Category>, ICategoryRepository
    {
        public CategoryRepository(DBContext _context) : base(_context)
        {
        }
    }
}