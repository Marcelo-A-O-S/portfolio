using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface ISocialAccountRepository : IGenerics<SocialAccount>
    {
        Task<SocialAccount> GetByProviderId(string providerId);
    }
}