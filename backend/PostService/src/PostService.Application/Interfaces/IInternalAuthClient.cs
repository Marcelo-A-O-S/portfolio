namespace PostService.Application.Interfaces
{
    public interface IInternalAuthClient
    {
        Task<string> GetToken();
    }
}