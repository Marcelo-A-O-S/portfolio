using CertificateService.Application.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace CertificateService.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext context;
        private IDbContextTransaction? transaction;
        public UnitOfWork(DBContext _context)
        {
            this.context = _context;
        }
        public async Task BeginAsync()
        {
            this.transaction = await this.context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
            if(this.transaction != null)
                await this.transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if(transaction != null)
                await this.transaction.RollbackAsync();
        }
    }
}