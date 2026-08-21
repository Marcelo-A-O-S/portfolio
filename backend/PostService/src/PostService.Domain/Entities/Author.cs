namespace PostService.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string ProfileUrl { get; private set; }
        public string ProviderId { get; private set; }
        public string Provider { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Author(Guid userId, string username, string profileUrl, string providerId, string provider, string? description)
        {
            this.UserId = userId;
            this.Username = username;
            this.ProfileUrl = profileUrl;
            this.ProviderId = providerId;
            this.Provider = provider;
            this.Description = description;
            this.CreatedAt = DateTime.UtcNow;
        }
        public void GenerateId()
        {
            this.Id = Guid.NewGuid();
        }
    }
}