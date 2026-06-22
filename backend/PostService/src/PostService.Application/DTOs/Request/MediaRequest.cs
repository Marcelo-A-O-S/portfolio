namespace PostService.Application.DTOs.Request
{
    public class MediaRequest
    {
        public Guid? Id { get; set; }
        public Guid MediaId { get; set; }
        public string Url { get; set; }
    }
}