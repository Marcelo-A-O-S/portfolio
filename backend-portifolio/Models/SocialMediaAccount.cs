using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_portifolio.Models
{
    public class SocialMediaAccount
    {
        public long Id { get; set; }
        public string Provider { get; set; } 
        public string SocialId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public SocialMediaAccount(string provider, string socialId, string email, string username)
        {
            Provider = provider;
            SocialId = socialId;
            Email = email;
            Username = username;
        }
    }
}