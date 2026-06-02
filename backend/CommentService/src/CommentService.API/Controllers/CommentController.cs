using CommentService.Application.UseCases.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CommentService.Application.DTOs.Request;
using CommentService.Application.DTOs.Request;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using CommentService.Application.UseCases.Likes.Interfaces;

namespace CommentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IAddComment addComment;
        private readonly IUpdateComment updateComment;
        private readonly IRemoveComment removeComment;
        private readonly IAddReply addReply;
        private readonly IUpdateReply updateReply;
        private readonly IRemoveReply removeReply;
        private readonly IAddLike addLike;
        private readonly IRemoveLike removeLike;
        public CommentController(
            IAddComment _addComment,
            IUpdateComment _updateComment,
            IRemoveComment _removeComment,
            IAddReply _addReply,
            IUpdateReply _updateReply,
            IRemoveReply _removeReply,
            IAddLike _addLike,
            IRemoveLike _removeLike
        )
        {
            this.addComment = _addComment;
            this.updateComment = _updateComment;
            this.removeComment = _removeComment;
            this.addReply = _addReply;
            this.updateReply = _updateReply;
            this.removeReply = _removeReply;
            this.addLike = _addLike;
            this.removeLike = _removeLike;
        }
        [HttpPost]
        [Authorize( Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddComment(CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized(new { message = "Usuário não autorizado."});
                await this.addComment.ExecuteAsync(Guid.Parse(userId), commentRequest);
                return Ok(new { message = "Comentário adicionado com sucesso!"});
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid Id, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized(new { message = "Usuário não autorizado."});
                await this.updateComment.ExecuteAsync(Guid.Parse(userId), Id, commentRequest);
                return Ok(new { message = "Comentário atualizado com sucesso!"});
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid Id)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized(new { message = "Usuário não autorizado."});
            await this.removeComment.ExecuteAsync(Guid.Parse(userId), Id);
            return Ok(new { message = "Comentário deletado com sucesso!"});
        }
        [HttpPost("{Id:guid}/Reply")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddReply([FromRoute] Guid Id, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized(new { message = "Usuário não autorizado."});
                await this.addReply.ExecuteAsync(Guid.Parse(userId), Id, commentRequest);
                return Ok(new { message = "Comentário adicionado com sucesso!"});
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateReply([FromRoute] Guid commentId, [FromRoute] Guid replyId, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if(userId == null)
                    return Unauthorized(new { message = "Usuário não autorizado."});
                await this.updateReply.ExecuteAsync(Guid.Parse(userId), commentId, replyId, commentRequest);
                return Ok(new { message = "Comentário atualizado com sucesso!"});
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteReply([FromRoute] Guid commentId, [FromRoute] Guid replyId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized(new { message = "Usuário não autorizado."});
            await this.removeReply.ExecuteAsync(Guid.Parse(userId), commentId, replyId);
            return Ok(new { message = "Comentário deletado com sucesso!"});
        }
        [HttpPost("{commentId:guid}/Like")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddLike([FromRoute] Guid commentId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.addLike.ExecuteAsync(Guid.Parse(userId), commentId);
            return Ok();
        }
        [HttpDelete("{commentId:guid}/Like")]
        [Authorize( Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> RemoveLike([FromRoute] Guid commentId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.removeLike.ExecuteAsync(Guid.Parse(userId), commentId);
            return Ok();
        }
    }
}