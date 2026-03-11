using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly IJwtBearerServices jwtBearerServices;
        private readonly ISocialAccountServices socialAccountServices;
        private readonly IRefreshTokenServices refreshTokenServices;
        public AuthController(
            IUserServices _userServices,
            IJwtBearerServices _jwtBearerServices,
            ISocialAccountServices _socialAccountServices,
            IRefreshTokenServices _refreshTokenServices)
        {
            this.userServices = _userServices;
            this.jwtBearerServices = _jwtBearerServices;
            this.socialAccountServices = _socialAccountServices;
            this.refreshTokenServices = _refreshTokenServices;
        }
        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userServices.FindBy(x => x.Email == loginRequest.Email);
                if (user == null)
                {
                    user = new User(loginRequest.Email, loginRequest.Name);
                    await this.userServices.Save(user);
                }
                var socialAccount = await this.socialAccountServices.GetByProviderId(loginRequest.ProviderId);
                if (socialAccount == null)
                {
                    socialAccount = new SocialAccount(
                        user.Id, loginRequest.Username,
                        loginRequest.ProfileUrl,
                        loginRequest.ProviderId,
                        loginRequest.Provider
                        );
                    await this.socialAccountServices.Save(socialAccount);
                }
                var accessData = await this.jwtBearerServices.GenerateAccessToken(user);
                var data = await this.jwtBearerServices.GenerateRefreshToken(user.Id, loginRequest.DeviceId, loginRequest.DeviceName);
                return Ok(new AuthResponse
                {
                    UserId = user.Id,
                    AccessToken = accessData.token,
                    RefreshToken = data.plainToken,
                    ExpireIn = accessData.expireIn,
                    Role = user.Role.ToString(),
                    RefreshTokenId = data.entity.Id
                });
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
        {
            if (ModelState.IsValid)
            {
                var authResponse = await this.jwtBearerServices.RefreshAsync(refreshRequest.RefreshTokenId,refreshRequest.UserId, refreshRequest.RefreshToken, refreshRequest.DeviceId, refreshRequest.DeviceName);
                return Ok(authResponse);
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest logoutRequest)
        {
            if (ModelState.IsValid)
            {
                var refreshToken = await this.refreshTokenServices.FindBy(rt => rt.DeviceId == logoutRequest.DeviceId && rt.UserId == logoutRequest.UserId);
                await this.refreshTokenServices.Delete(refreshToken);
                return Ok();
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
    }
}