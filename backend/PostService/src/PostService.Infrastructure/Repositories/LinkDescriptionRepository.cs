using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class LinkDescriptionRepository : Generics<LinkDescription>, ILinkDescriptionRepository
    {
        public LinkDescriptionRepository(DBContext _context) : base(_context)
        {
        }
    }
}