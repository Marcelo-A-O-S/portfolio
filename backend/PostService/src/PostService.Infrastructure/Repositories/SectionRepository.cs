using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class SectionRepository : Generics<Section>, ISectionRepository
    {
        public SectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}