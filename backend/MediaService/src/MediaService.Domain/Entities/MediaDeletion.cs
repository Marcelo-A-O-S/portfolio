namespace MediaService.Domain.Entities
{
    public class MediaDeletion
    {
        public Guid Id { get; private set; }
        public string Path { get; private set; }
        public int Attempts { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public MediaDeletion(
            string path
        )
        {
            this.Path = path;
            this.Attempts = 0;
            this.CreatedAt = DateTime.UtcNow;
        }
        public void Processed()
        {
            this.ProcessedAt = DateTime.UtcNow;
            this.Attempts = 0;
        }
        public void Failure()
        {
            this.Attempts++;
        }
    }
}