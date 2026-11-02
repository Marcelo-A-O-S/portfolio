using PostService.Domain.Enums;
namespace PostService.Domain.Entities
{
    public class OutboxMessage
    {
        private const int MaxAttempts = 10;

        public Guid Id { get; private set; }
        public string EventType { get; private set; }
        public string Payload { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public DateTime? ProcessingStartedAt { get; private set; }
        public string? Error { get; private set; }
        public int Attempts { get; private set; }
        public OutboxStatus Status { get; private set; }
        protected OutboxMessage() { }
        public OutboxMessage(string eventType, string payload)
        {
            ArgumentException.ThrowIfNullOrEmpty(eventType);
            ArgumentException.ThrowIfNullOrEmpty(payload);
            Id = Guid.NewGuid();
            EventType = eventType;
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
            Attempts = 0;
            Status = OutboxStatus.Pending;
        }
        public void MarkAsProcessing()
        {
            if (Status != OutboxStatus.Pending)
                throw new InvalidOperationException(
                    "Somente mensagens pendentes podem ser processadas.");
            if (Attempts >= MaxAttempts)
                throw new InvalidOperationException(
                    "A mensagem atingiu o limite de tentativas.");
            Attempts++;
            Status = OutboxStatus.Processing;
            ProcessingStartedAt = DateTime.UtcNow;
        }
        public void MarkAsProcessed()
        {
            if (Status != OutboxStatus.Processing)
                throw new InvalidOperationException(
                    "A mensagem precisa estar em processamento.");
            ProcessedAt = DateTime.UtcNow;
            ProcessingStartedAt = null;
            Status = OutboxStatus.Processed;
            Error = null;
        }
        public void RegisterFailure(string error)
        {
            ArgumentException.ThrowIfNullOrEmpty(error);
            if (Status != OutboxStatus.Processing)
                throw new InvalidOperationException(
                    "Somente mensagens em processamento podem registrar falhas.");
            Error = error;
            ProcessingStartedAt = null;
            Status = Attempts >= MaxAttempts
                ? OutboxStatus.Failed
                : OutboxStatus.Pending;
        }
    }
}