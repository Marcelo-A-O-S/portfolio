namespace AuthService.Application.UseCases.InternalUser.Interfaces
{
    public interface IExistsProviderId
    {
        Task<bool> ExecuteAsync(Guid userId, string providerId);
    }
}