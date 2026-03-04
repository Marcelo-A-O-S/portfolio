using AuthService.Application.DTOs.Response;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Application.Services
{
    public class JwtBearerServices : IJwtBearerServices
    {
        private readonly IConfiguration configuration;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUserRepository userRepository;
        public JwtBearerServices(IConfiguration _configuration, IRefreshTokenRepository _refreshTokenRepository, IUserRepository _userRepository)
        {
            this.configuration = _configuration;
            this.refreshTokenRepository = _refreshTokenRepository;
            this.userRepository = _userRepository;
        }
        public async Task<(string token, int expireIn)> GenerateAccessToken(User user)
        {
            var Secret = this.configuration.GetSection("JWT:Secret").Value;
            var IssuerSecret = this.configuration.GetSection("JWT:Issuer").Value;
            if (string.IsNullOrEmpty(Secret) || string.IsNullOrEmpty(IssuerSecret))
            {
                throw new Exception("Chaves jwt não configuradas corretamente.");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expirationMinutes = 15;
            var expireIn = (int)TimeSpan.FromMinutes(expirationMinutes).TotalSeconds;
            var expirationDate = DateTime.UtcNow.AddSeconds(expireIn);
            var token = new JwtSecurityToken(
                issuer: IssuerSecret,
                claims: claims,
                expires: expirationDate,
                signingCredentials: credentials
            );
            return (new JwtSecurityTokenHandler().WriteToken(token), expireIn);
        }
        public async Task<(RefreshToken entity, string plainToken)> GenerateRefreshToken(Guid userId, string deviceId, string deviceName)
        {
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
            return (entity, plainToken);
        }
        public async Task<AuthResponse> RefreshAsync(Guid userId, string refreshToken, string deviceId)
        {
            var user = await this.userRepository.GetById(userId);
            if(user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            var token = await this.refreshTokenRepository.FindBy(rt => rt.UserId == userId && rt.DeviceId == deviceId);
            if (token == null)
            {
                throw new Exception("Refresh token inválido.");
            }
            if (!BCrypt.Net.BCrypt.Verify(refreshToken, token.TokenHash))
            {
                throw new Exception("Refresh token inválido.");
            }
            if (token.IsExpired)
            {
                await this.refreshTokenRepository.Delete(token);
                throw new Exception("Refresh token expirado. Gentileza realizar o login novamente.");
            }
            if (token.IsRevoked)
            {
                throw new Exception("Refresh token revogado.");
            }
            var newAccessToken = await this.GenerateAccessToken(token.User);
            return new AuthResponse
            {
                UserId = userId,
                AccessToken = newAccessToken.token,
                RefreshToken = refreshToken,
                ExpireIn = newAccessToken.expireIn,
                Role = user.Role.ToString()
            };
        }
    }
}