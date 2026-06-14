namespace MediaService.Application.UseCases.MediaFiles.Interfaces
{
    public interface IRemoveMediaFile
    {
        Task ExecuteAsync(Guid mediaFileId);
    }
}