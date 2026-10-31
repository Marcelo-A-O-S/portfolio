namespace MediaService.Domain.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string EventType { get; private set; }
        public string Payload { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? Error { get; private set; }
        public int Attempts { get; private set; }

        protected OutboxMessage() { }

        public OutboxMessage(string eventType, string payload)
        {
            Id = Guid.NewGuid();
            EventType = eventType;
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
            Attempts = 0;
        }

        public void MarkAsProcessed()
        {
            ProcessedAt = DateTime.UtcNow;
            Error = null;
        }

        public void RegisterFailure(string error)
        {
            Attempts++;
            Error = error;
        }
    }
}