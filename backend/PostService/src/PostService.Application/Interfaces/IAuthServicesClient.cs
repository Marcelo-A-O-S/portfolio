namespace PostService.Application.Interfaces
{
    public interface IAuthServicesClient
    {
        Task<string> GetToken();
    }
}