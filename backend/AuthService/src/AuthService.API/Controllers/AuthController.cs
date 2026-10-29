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
        private readonly IRefreshToken refreshToken;
        private readonly ILogout logout;
        public AuthController(
            ILogin _login,
            IRefreshToken _refreshToken,
            ILogout _logout)
        {
            this.login = _login;
            this.refreshToken = _refreshToken;
            this.logout = _logout;
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
                var authResponse = await this.refreshToken.ExecuteAsync(refreshRequest);
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
                await this.logout.ExecuteAsync(logoutRequest);
                return Ok();
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
    }
}