using System.Linq.Expressions;
using System.Linq.Expressions;
namespace PostService.Application.Interfaces
{
    public interface IServices<T>
    {
        Task Save(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeleteById(Guid Id);
        Task<T> FindBy(Expression<Func<T,bool>> predicate);
        Task<T> GetById(Guid Id);
        Task<List<T>> List();
        Task<List<T>> List(int page);
    }
}