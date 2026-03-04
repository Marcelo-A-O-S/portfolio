using PostService.Domain.Interfaces;
using PostService.Domain.Entities;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class ToolsRepository : Generics<Tool>, IToolsRepository
    {
        public ToolsRepository(DBContext _context) : base(_context)
        {
        }
    }
}