namespace MediaService.Application.DTOs.Responses
{
    public class MediaFileResponse
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string OwnerType { get; set; }
    }
}