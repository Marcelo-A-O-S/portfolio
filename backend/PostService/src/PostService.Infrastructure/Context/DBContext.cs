using Microsoft.EntityFrameworkCore;
namespace PostService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
        
    }
}