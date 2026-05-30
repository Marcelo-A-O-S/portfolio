using CommentService.Application.UseCases.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CommentService.Application.DTOs.Request;
using CommentService.Application.DTOs.Request;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CommentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IAddComment addComment;
        private readonly IUpdateComment updateComment;
        private readonly IRemoveComment removeComment;
        public CommentController(
            IAddComment _addComment,
            IUpdateComment _updateComment,
            IRemoveComment _removeComment
        )
        {
            this.addComment = _addComment;
            this.updateComment = _updateComment;
            this.removeComment = _removeComment;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized();
                await this.addComment.ExecuteAsync(Guid.Parse(userId), commentRequest);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid Id, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized();
                await this.updateComment.ExecuteAsync(Guid.Parse(userId), Id, commentRequest);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid Id)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.removeComment.ExecuteAsync(Guid.Parse(userId), Id);
            return Ok();
        }
    }
}