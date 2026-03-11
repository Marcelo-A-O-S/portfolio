using AuthService.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public Role Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<SocialAccount> SocialAccounts { get; private set; }
        public UserStatus Status { get; private set; }
        public User(string email, string name)
        {
            this.Id = Guid.NewGuid();
            this.Email = email;
            this.Name = name;
            this.Role = Role.Client;
            this.CreatedAt = DateTime.UtcNow;
            this.Status = UserStatus.ACTIVE;
        }
        public void Update(string email, string name, Role role, DateTime dateTime)
        {
            this.Email = email;
            this.Name = name;
            this.Role = role;
            this.CreatedAt = dateTime;
        }
        public void UpdateRole(Role role)
        {
            this.Role = role;
        }
        public void Ban()
        {
            this.Status = UserStatus.BANNED;
        }
        public async Task ValidateEmail()
        {
            var host = new Uri($"mailto:{this.Email}").Host;
            var result = await Dns.GetHostEntryAsync(host);
            if(result == null)
            {
                throw new Exception("Dominio de email inválido");
            }
        }
    }
}