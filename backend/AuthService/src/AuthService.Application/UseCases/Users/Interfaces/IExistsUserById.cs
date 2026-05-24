namespace AuthService.Application.UseCases.Users.Interfaces
{
    public interface IExistsByIdUser
    {
        Task<bool> ExecuteAsync(Guid userId);
    }
}