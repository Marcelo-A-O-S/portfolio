namespace AuthService.Application.Interfaces
{
    public interface IProviderFactory
    {
        IProviderValidationService Get(string provider);
    }
}