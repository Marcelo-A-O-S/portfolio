using AuthService.Application.Configurations;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace AuthService.Application.Services
{
    public class JwtBearerServices : IJwtBearerServices
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUserRepository userRepository;
        private readonly InternalJWTOptions jwtInternalOptions;
        private readonly JwtBearerOptions jwtBearerOptions;
        public JwtBearerServices(
            IRefreshTokenRepository _refreshTokenRepository, 
            IUserRepository _userRepository,
            IOptions<InternalJWTOptions> _jwtInternalOptions,
            IOptions<JwtBearerOptions> _jwtBearerOptions)
        {
            this.refreshTokenRepository = _refreshTokenRepository;
            this.userRepository = _userRepository;
            this.jwtInternalOptions = _jwtInternalOptions.Value;
            this.jwtBearerOptions = _jwtBearerOptions.Value;
        }
        public async Task<(string token, int expireIn)> GenerateAccessToken(User user)
        {
            if (string.IsNullOrEmpty(this.jwtBearerOptions.Secret) || string.IsNullOrEmpty(this.jwtBearerOptions.Issuer))
            {
                throw new Exception("Chaves jwt não configuradas corretamente.");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim("Status", user.Status.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtBearerOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireIn = (int)TimeSpan.FromMinutes(this.jwtBearerOptions.ExpirationMinutes).TotalSeconds;
            var expirationDate = DateTime.UtcNow.AddSeconds(expireIn);
            var token = new JwtSecurityToken(
                issuer: this.jwtBearerOptions.Issuer,
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials
            );
            return (new JwtSecurityTokenHandler().WriteToken(token), expireIn);
        }
        public async Task<string> GenerateInternalToken(List<Claim> claims)
        {
            if(this.jwtInternalOptions == null)
                throw new Exception("Chaves jwt não configuradas corretamente.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtInternalOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireIn = (int)TimeSpan.FromMinutes(this.jwtInternalOptions.InternalExpirationMinutes).TotalSeconds;
            var expirationDate = DateTime.UtcNow.AddSeconds(expireIn);
            var token = new JwtSecurityToken(
                issuer: this.jwtInternalOptions.Issuer,
                audience: "auth-internal",
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<(RefreshToken entity, string plainToken)> GenerateRefreshToken(Guid userId, string deviceId, string deviceName)
        {
            var existing = await this.refreshTokenRepository.FindBy(rt => rt.UserId == userId && rt.DeviceId == deviceId && rt.RevokedAt == null);
            if (existing != null)
            {
                existing.Revoke();
                await this.refreshTokenRepository.Update(existing);
            }
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var plainToken = Convert.ToBase64String(randomBytes);
            var hash = BCrypt.Net.BCrypt.HashPassword(plainToken);
            var entity = new RefreshToken(
                userId,
                hash,
                deviceId,
                deviceName,
                DateTime.UtcNow.AddDays(7)
            );
            await this.refreshTokenRepository.Save(entity);
            return (entity, plainToken);
        }
        public async Task<AuthResponse> RefreshAsync(Guid refreshTokenId, Guid userId, string refreshToken, string deviceId, string deviceName)
        {
            var user = await this.userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            var token = await this.refreshTokenRepository.FindBy(rt => rt.Id == refreshTokenId && rt.UserId == userId && rt.DeviceId == deviceId && rt.RevokedAt == null);
            if (token == null)
            {
                throw new Exception("Refresh token inválido.");
            }
            if (token.IsExpired)
            {
                await this.refreshTokenRepository.Delete(token);
                throw new Exception("Refresh token expirado. Gentileza realizar o login novamente.");
            }
            if (!BCrypt.Net.BCrypt.Verify(refreshToken, token.TokenHash))
            {
                throw new Exception("Refresh token inválido.");
            }
            if (token.IsRevoked)
            {
                throw new Exception("Refresh token reutilizado.");
            }
            var newRefresh = await this.GenerateRefreshToken(userId, deviceId, deviceName);
            var newAccessToken = await this.GenerateAccessToken(token.User);
            return new AuthResponse
            {
                UserId = userId,
                AccessToken = newAccessToken.token,
                RefreshToken = newRefresh.plainToken,
                ExpireIn = newAccessToken.expireIn,
                Role = user.Role.ToString(),
                RefreshTokenId = newRefresh.entity.Id,
            };
        }
    }
}