using Microsoft.EntityFrameworkCore;

namespace CommentService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
    }
}