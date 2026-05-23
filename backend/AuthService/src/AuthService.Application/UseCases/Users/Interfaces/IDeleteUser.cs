namespace AuthService.Application.UseCases.Users.Interfaces
{
    public interface IDeleteUser
    {
        Task ExecuteAsync(Guid Id);
    }
}