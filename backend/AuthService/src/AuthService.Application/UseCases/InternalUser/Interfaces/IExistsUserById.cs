namespace AuthService.Application.UseCases.InternalUser.Interfaces
{
    public interface IExistsByIdUser
    {
        Task<bool> ExecuteAsync(Guid userId);
    }
}