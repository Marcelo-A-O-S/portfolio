namespace PostService.Domain.Entities
{
    public class MediaProjection
    {
        public Guid Id { get; set; }
        public Guid MediaId { get; set;}
        public string Url { get; set; }
        public MediaProjection(
            Guid mediaId,
            string url
        )
        {
            this.MediaId = mediaId;
            this.Url = url;
        }
    }
}