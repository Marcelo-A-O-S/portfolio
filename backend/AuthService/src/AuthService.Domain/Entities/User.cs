using AuthService.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<SocialAccount> SocialAccounts { get; set; }
        public User(string email, string name)
        {
            this.Id = Guid.NewGuid();
            this.Email = email;
            this.Name = name;
            this.Role = Role.Client;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}