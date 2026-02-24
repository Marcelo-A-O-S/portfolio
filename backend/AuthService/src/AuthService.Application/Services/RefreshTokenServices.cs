using System.Linq.Expressions;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
namespace AuthService.Application.Services
{
    public class RefreshTokenServices : IRefreshTokenServices
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        public RefreshTokenServices(IRefreshTokenRepository _refreshTokenRepository)
        {
            this.refreshTokenRepository = _refreshTokenRepository;
        }
        public async Task CleanupExpiredTokensAsync()
        {
            await this.refreshTokenRepository.DeleteExpiredAsync();
        }
        public async Task Delete(RefreshToken entity)
        {
            await this.refreshTokenRepository.Delete(entity);
        }
        public async Task<RefreshToken> FindBy(Expression<Func<RefreshToken, bool>> predicate)
        {
            return await this.refreshTokenRepository.FindBy(predicate);
        }
        public async Task<RefreshToken> GetById(Guid Id)
        {
            return await this.refreshTokenRepository.GetById(Id);
        }
        public async Task<List<RefreshToken>> List()
        {
            return await this.refreshTokenRepository.List();
        }
        public async Task<List<RefreshToken>> List(int page)
        {
            return await this.refreshTokenRepository.List(page);
        }
        public async Task Save(RefreshToken entity)
        {
            await this.refreshTokenRepository.Save(entity);
        }
        public async Task Update(RefreshToken entity)
        {
            await this.refreshTokenRepository.Update(entity);
        }
    }
}