using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : Generics<RefreshToken>, IRefreshTokenRepository
    {
        private readonly DBContext context;
        public RefreshTokenRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task DeleteExpiredAsync()
        {
            await this.context.RefreshTokens.Where(rt => rt.ExpiresAt < DateTime.UtcNow).ExecuteDeleteAsync();    
        }
        public async Task<RefreshToken> GetByDeviceId(string deviceId)
        {
            return await this.context.RefreshTokens.Where(rt => rt.DeviceId == deviceId).FirstOrDefaultAsync();
        }
    }
}