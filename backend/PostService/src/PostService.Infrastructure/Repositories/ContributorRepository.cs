using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class ContributorRepository : Generics<Contributor>, IContributorRepository
    {
        public ContributorRepository(DBContext _context) : base(_context)
        {
        }
    }
}