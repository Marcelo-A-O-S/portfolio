namespace AuthService.Application.UseCases.Users.Interfaces
{
    public interface IBanUser
    {
        Task ExecuteAsync(Guid Id);
    }
}