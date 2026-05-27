using AuthService.Application.DTOs.Request;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogin login;
        private readonly IJwtBearerServices jwtBearerServices;
        private readonly IRefreshTokenServices refreshTokenServices;
        public AuthController(
            IJwtBearerServices _jwtBearerServices,
            IRefreshTokenServices _refreshTokenServices,
            ILogin _login)
        {
            this.jwtBearerServices = _jwtBearerServices;
            this.refreshTokenServices = _refreshTokenServices;
            this.login = _login;
        }
        [HttpPost("login")]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var authResponse = await this.login.ExecuteASync(loginRequest);
                return Ok(authResponse);
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