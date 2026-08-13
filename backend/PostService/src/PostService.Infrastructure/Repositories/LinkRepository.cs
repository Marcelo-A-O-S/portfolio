using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LinkRepository : Generics<Link>, ILinkRepository
    {
        public LinkRepository(DBContext _context) : base(_context)
        {
        }
    }
}