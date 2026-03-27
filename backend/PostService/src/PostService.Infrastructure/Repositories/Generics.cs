using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

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
            context.ChangeTracker.TrackGraph(entity, e =>
            {
                if (e.Entry.Entity == entity)
                {
                    e.Entry.State = EntityState.Added;
                    return;
                }
                e.Entry.State = e.Entry.State == EntityState.Detached
                    ? EntityState.Added
                    : EntityState.Unchanged;
            });
            await this.context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            context.ChangeTracker.TrackGraph(entity, e =>
            {
                if (e.Entry.Entity == entity)
                {
                    e.Entry.State = EntityState.Modified;
                    return;
                }
                e.Entry.State = e.Entry.IsKeySet
                    ? EntityState.Unchanged
                    : EntityState.Added;
            });
            await this.context.SaveChangesAsync();
        }
    }
}