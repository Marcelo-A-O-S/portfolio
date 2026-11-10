namespace MediaService.Application.DTOs.Requests
{
    public class MediaDeleteRequest
    {
        public Guid MediaId { get; set; }
        public string OwnerType { get; set; } 
    }
}