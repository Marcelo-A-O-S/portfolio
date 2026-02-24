using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace AuthService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id {get; private set;}

        public Guid UserId {get; private set;}
        public User User { get; private set; }
        public string TokenHash { get; private set;}
        public string DeviceId { get; private set;}
        public string DeviceName { get; private set;}
        public DateTime CreatedAt { get; private set;}
        public DateTime ExpiresAt { get; private set; }

        public DateTime? RevokedAt { get; private set;}
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsExpired && !IsRevoked;
        protected RefreshToken() {}
        public RefreshToken(
            Guid userId,
            string tokenHash,
            string deviceId,
            string deviceName,
            DateTime expiresAt
        )
        {
            this.Id = Guid.NewGuid();
            this.UserId = userId;
            this.TokenHash = tokenHash;
            this.DeviceId = deviceId;
            this.DeviceName = deviceName;
            this.CreatedAt = DateTime.UtcNow;
            this.ExpiresAt = expiresAt;
        }
        public void Revoke()
        {
            this.RevokedAt = DateTime.UtcNow;
        }
    }
}