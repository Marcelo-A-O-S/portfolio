using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class ToolContentRepository : Generics<ToolContent>, IToolContentRepository
    {
        private readonly DBContext context;
        public ToolContentRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<ToolContent> GetByToolId(Guid toolId)
        {
            return await this.context.ToolContents.Where(tl => tl.ToolId == toolId).FirstOrDefaultAsync();
        }
    }
}