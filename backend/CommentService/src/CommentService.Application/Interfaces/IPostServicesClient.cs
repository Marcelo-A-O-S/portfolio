namespace CommentService.Application.Interfaces
{
    public interface IPostServicesClient
    {
        Task<bool> PostExistsAsync(Guid postId);
    }
}