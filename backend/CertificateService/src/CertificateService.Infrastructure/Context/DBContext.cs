using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
    }
}