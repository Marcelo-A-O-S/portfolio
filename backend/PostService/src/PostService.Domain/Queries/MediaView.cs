namespace PostService.Domain.Queries
{
    public class MediaView
    {
        public Guid? Id { get; set; }
        public Guid MediaId { get; set; }
        public string Url { get; set; }
    }
}