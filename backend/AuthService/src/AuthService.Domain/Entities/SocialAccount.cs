namespace AuthService.Domain.Entities
{
    public class SocialAccount
    {
        public Guid Id { get; private set;}
        public Guid UserId { get; private set;}
        public string Username { get; private set;}
        public string? ProfileUrl { get; private set;}
        public string ProviderId {get; private set;}
        public string Provider {get; private set; }
        public bool VerifiedAccount { get; private set; }
        public SocialAccount(Guid userId, string username, string profileUrl, string providerId, string provider, bool verifiedAccount)
        {
            this.Id = Guid.Empty;
            this.UserId = userId;
            this.Username = username;
            this.ProfileUrl = profileUrl;
            this.ProviderId = providerId;
            this.Provider = provider;
            this.VerifiedAccount = verifiedAccount;
            
        }
    }
}