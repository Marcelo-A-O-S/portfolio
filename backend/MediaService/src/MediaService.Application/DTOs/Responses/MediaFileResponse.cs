namespace MediaService.Application.DTOs.Responses
{
    public class MediaFileResponse
    {
        public Guid MediaId { get; set; }
        public string Url { get; set; }
        public string OwnerType { get; set; }
    }
}