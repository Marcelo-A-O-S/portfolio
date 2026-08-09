using PostService.Application.Interfaces;
using PostService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PostService.Application.Exceptions;
namespace PostService.Infrastructure.Persistence
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
            try
            {
                await this.context.SaveChangesAsync();
                if(this.transaction != null)
                    await this.transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException postgresEx
                    && postgresEx.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    throw new DuplicateException();
                }
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            if(transaction != null)
                await this.transaction.RollbackAsync();
        }
    }
}