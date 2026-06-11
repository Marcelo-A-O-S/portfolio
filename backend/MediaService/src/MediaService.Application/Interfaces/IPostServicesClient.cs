namespace MediaService.Application.Interfaces
{
    public interface IPostServicesClient
    {
        Task<bool> PostExistsAsync(Guid postId);
    }
}