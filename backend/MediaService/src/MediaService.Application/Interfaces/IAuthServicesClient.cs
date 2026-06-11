namespace MediaService.Application.Interfaces
{
    public interface IAuthServicesClient
    {
        Task<string> GetToken();
    }
}