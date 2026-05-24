using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PostService.Application.Exceptions;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
using PostService.Domain.Entities;

namespace PostService.Infrastructure.Repositories
{
    public class Generics<T> : IGenerics<T> where T : class
    {
        private readonly DBContext context;
        public Generics(DBContext _context)
        {
            this.context = _context;
        }
        public async Task Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await this.context.SaveChangesAsync();
        }
        public async Task DeleteById(Guid Id)
        {
            var entity = await this.context.Set<T>().FindAsync(Id);
            this.context.Set<T>().Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await this.context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await this.context.Set<T>().FindAsync(Id);
        }

        public async Task<List<T>> List()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> List(int page = 1, int itemsPage = 10)
        {
            var query = this.context.Set<T>().AsNoTracking().AsQueryable();
            var items = await query.Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .ToListAsync();
            return items;
        }
        public async Task Save(T entity)
        {
            try
            {
                await this.context.AddAsync(entity);
                await this.context.SaveChangesAsync();
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

        public async Task Update(T entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();
        }
    }
}