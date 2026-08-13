using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LinkTypeRepository : Generics<LinkType>, ILinkTypeRepository
    {
        public LinkTypeRepository(DBContext _context) : base(_context)
        {
        }
    }
}