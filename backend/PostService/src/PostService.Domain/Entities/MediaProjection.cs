namespace PostService.Domain.Entities
{
    public class MediaProjection
    {
        public Guid Id { get; set; }
        public Guid MediaFileId { get; set;}
        public Guid OwnerId { get; set; }
        public string Url { get; set; }
    }
}