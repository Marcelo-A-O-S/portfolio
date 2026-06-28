using CommentService.Application.DTOs.Request;
using CommentService.Application.UseCases.Likes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly IAddLike addLike;
        private readonly IRemoveLike removeLike;
        public LikeController(
            IAddLike _addLike,
            IRemoveLike _removeLike
        )
        {
            this.addLike = _addLike;
            this.removeLike = _removeLike;
        }
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.addLike.ExecuteAsync(Guid.Parse(userId), likeRequest);
            return Ok();
        }
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> RemoveLike([FromBody] LikeRequest likeRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.removeLike.ExecuteAsync(Guid.Parse(userId), likeRequest);
            return Ok();
        }
    }
}