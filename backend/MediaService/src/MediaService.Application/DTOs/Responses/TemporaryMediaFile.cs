namespace MediaService.Application.DTOs.Responses
{
    public record TemporaryMediaFile(
        string TemporaryPath,
        string FinalPath,
        string ContentType,
        long Length
    );
}