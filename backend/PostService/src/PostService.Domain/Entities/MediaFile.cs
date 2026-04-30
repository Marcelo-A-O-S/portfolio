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
        public MediaFile(string path, string mimeType, long size, bool isCommitted)
        {
            this.Path = path;
            this.MimeType = mimeType;
            this.Size = size;
            this.IsCommitted = isCommitted;
            this.CreatedAt = DateTime.UtcNow;
            this.CommittedAt = isCommitted ? DateTime.UtcNow : null;
        }
        public void UpdatePath(string path)
        {
            this.Path = path;
        }
        public void Commit()
        {
            this.IsCommitted = true;
            this.CommittedAt = DateTime.UtcNow;
        }
    }
}