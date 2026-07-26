using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class SocialAccountRepository : Generics<SocialAccount>, ISocialAccountRepository
    {
        private readonly DBContext context;
        public SocialAccountRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<SocialAccount> GetByProviderId(string providerId)
        {
            return await this.context.SocialAccounts.Where(s=> s.ProviderId == providerId).FirstOrDefaultAsync();
        }

        public async Task<bool> VerifyProviderExists(Guid userId, string providerId)
        {
            var socialAccount = await this.context.SocialAccounts.Where(s => s.UserId == userId && s.ProviderId == providerId).FirstOrDefaultAsync();
            if(socialAccount == null)
                return false;
            return true;
        }
    }
}