namespace CommentService.Application.Interfaces
{
    public interface IInternalAuthClient
    {
        Task<string> GetToken();
    }
}