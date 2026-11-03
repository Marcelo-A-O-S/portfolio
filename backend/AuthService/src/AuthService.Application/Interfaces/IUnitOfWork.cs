namespace AuthService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task ReloadAsync<T>(T entity) where T: class;
    }
}