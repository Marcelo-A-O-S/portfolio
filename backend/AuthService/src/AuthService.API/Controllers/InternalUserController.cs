using AuthService.Application.UseCases.InternalUser.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalUserController : ControllerBase
    {
        private readonly IExistsByIdUser existsByIdUser;
        private readonly IGetByIdUser getByIdUser;
        private readonly IExistsProviderId existsProviderId;
        public InternalUserController(
            IExistsByIdUser _existsByIdUser,
            IGetByIdUser _getByIdUser,
            IExistsProviderId _existsProviderId
        )
        {
            this.existsByIdUser = _existsByIdUser;
            this.getByIdUser = _getByIdUser;
            this.existsProviderId = _existsProviderId;
        }
        [HttpGet("{Id}/exists")]
        [Authorize(AuthenticationSchemes = "InternalJwt", Policy = "UsersRead")]
        public async Task<IActionResult> UserExists([FromRoute] Guid Id)
        {
            var exists = await existsByIdUser.ExecuteAsync(Id);
            if (!exists)
                return NotFound();
            return Ok(exists);
        }
        [HttpGet("{userId}/provider/{providerId}/exists")]
        [Authorize(AuthenticationSchemes = "InternalJwt", Policy = "UsersRead")]
        public async Task<IActionResult> ProviderExists([FromRoute] Guid userId, [FromRoute] string providerId)
        {
            var exists = await this.existsProviderId.ExecuteAsync(userId, providerId);
            if (!exists)
                return NotFound();
            return Ok(exists);
        }
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "InternalJwt", Policy = "UsersRead")]
        public async Task<IActionResult> GetUser([FromRoute] Guid Id)
        {
            return Ok();
        }
        [HttpGet("{Id:guid}/provider/{ProviderId}")]
        [Authorize(AuthenticationSchemes = "InternalJwt", Policy = "UsersRead")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid Id, [FromRoute] string ProviderId)
        {
            var user = await this.getByIdUser.ExecuteAsync(Id, ProviderId);
            return Ok(user);
        }
    }
}