using AuthService.Application.DTOs.Request;
using AuthService.Application.UseCases.InternalUser.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalAuthController : ControllerBase
    {
        private readonly ICreateToken createToken;
        public InternalAuthController(
            ICreateToken _createToken
        )
        {
            this.createToken = _createToken;
        }
        [HttpPost("token")]
        [EnableRateLimiting("internal-auth")]
        public async Task<IActionResult> GenerateToken(ServiceAuthRequest request)
        {
            var tokenResponse = await this.createToken.ExecuteAsync(request);
            return Ok(tokenResponse);
        }
    }
}