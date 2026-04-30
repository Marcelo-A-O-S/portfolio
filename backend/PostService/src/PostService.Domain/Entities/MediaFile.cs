namespace PostService.Domain.Entities
{
    public class MediaFile
    {
        public Guid Id { get; private set;}
        public string Path { get; private set; }
        public string MimeType { get; private set; }
        public long Size {get; private set; }
        public bool IsCommitted { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public DateTime? CommittedAt { get; private set; }
    }
}