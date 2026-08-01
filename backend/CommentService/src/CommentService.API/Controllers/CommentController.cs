using System.Security.Claims;
using CommentService.Application.DTOs.Request;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CommentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentServices commentServices;
        private readonly IAddComment addComment;
        private readonly IUpdateComment updateComment;
        private readonly IRemoveComment removeComment;
        private readonly IRemoveByUserComment removeByUserComment;
        private readonly IRemoveByModeratorComment removeByModeratorComment;
        private readonly IRemoveByAdminComment removeByAdminComment;
        private readonly IAddReply addReply;
        private readonly IUpdateReply updateReply;
        private readonly IRemoveReply removeReply;
        private readonly IRemoveByUserReply removeByUserReply;
        private readonly IRemoveByModeratorReply removeByModeratorReply;
        private readonly IRemoveByAdminReply removeByAdminReply;
        private readonly IAddLike addLike;
        private readonly IRemoveLike removeLike;
        public CommentController(
            IAddComment _addComment,
            IUpdateComment _updateComment,
            IRemoveComment _removeComment,
            IRemoveByUserComment _removeByUserComment,
            IRemoveByModeratorComment _removeByModeratorComment,
            IRemoveByAdminComment _removeByAdminComment,
            IAddReply _addReply,
            IUpdateReply _updateReply,
            IRemoveReply _removeReply,
            IRemoveByUserReply _removeByUserReply,
            IRemoveByModeratorReply _removeByModeratorReply,
            IRemoveByAdminReply _removeByAdminReply,
            IAddLike _addLike,
            IRemoveLike _removeLike,
            ICommentServices _commentServices
        )
        {
            this.addComment = _addComment;
            this.updateComment = _updateComment;
            this.removeComment = _removeComment;
            this.removeByUserComment = _removeByUserComment;
            this.removeByModeratorComment = _removeByModeratorComment;
            this.removeByAdminComment = _removeByAdminComment;
            this.addReply = _addReply;
            this.updateReply = _updateReply;
            this.removeReply = _removeReply;
            this.removeByUserReply = _removeByUserReply;
            this.removeByModeratorReply = _removeByModeratorReply;
            this.removeByAdminReply = _removeByAdminReply;
            this.addLike = _addLike;
            this.removeLike = _removeLike;
            this.commentServices = _commentServices;
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetPagination(
            [FromQuery] int page,
            [FromQuery] Guid targetId,
            [FromQuery] CommentType type)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await this.commentServices.GetCommentsPaginationByTargetAndType(Guid.Parse(userId), targetId, type, page);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddComment(CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.addComment.ExecuteAsync(Guid.Parse(userId), providerId, commentRequest);
                return Ok(new { message = "Comentário adicionado com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid Id, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.updateComment.ExecuteAsync(Guid.Parse(userId), role, Id, commentRequest);
                return Ok(new { message = "Comentário atualizado com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid Id)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null || role == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeComment.ExecuteAsync(Guid.Parse(userId), role, Id);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("User/{Id}")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteCommentByUser([FromRoute] Guid Id)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByUserComment.ExecuteAsync(Guid.Parse(userId), Id);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("Moderator/{Id}")]
        [Authorize(Roles = "Administrador,Moderator", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteCommentByModeration([FromRoute] Guid Id)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByModeratorComment.ExecuteAsync(Guid.Parse(userId), Id);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("Administration/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteCommentByAdministration([FromRoute] Guid Id)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByAdminComment.ExecuteAsync(Guid.Parse(userId), Id);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpPost("{Id:guid}/Reply")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddReply([FromRoute] Guid Id, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.addReply.ExecuteAsync(Guid.Parse(userId), providerId, Id, commentRequest);
                return Ok(new { message = "Comentário adicionado com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateReply([FromRoute] Guid commentId, [FromRoute] Guid replyId, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.updateReply.ExecuteAsync(Guid.Parse(userId), role, commentId, replyId, commentRequest);
                return Ok(new { message = "Comentário atualizado com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize(Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteReply([FromRoute] Guid commentId, [FromRoute] Guid replyId)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userId == null || role == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeReply.ExecuteAsync(Guid.Parse(userId), role, commentId, replyId);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("User/{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize(Roles = "Administrador,Moderator,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteReplyByUser([FromRoute] Guid commentId, [FromRoute] Guid replyId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByUserReply.ExecuteAsync(Guid.Parse(userId), commentId, replyId);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("Moderator/{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize(Roles = "Administrador,Moderator", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteReplyByModerator([FromRoute] Guid commentId, [FromRoute] Guid replyId)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByModeratorReply.ExecuteAsync(Guid.Parse(userId), commentId, replyId);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpDelete("Administration/{commentId:guid}/Reply/{replyId:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteReplyByAdministrador([FromRoute] Guid commentId, [FromRoute] Guid replyId)
        {
            var providerId = User.FindFirst("ProviderId")?.Value;
            if (providerId == null)
                return BadRequest(new { message = "Provider inválido." });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Usuário não autorizado." });
            await this.removeByAdminReply.ExecuteAsync(Guid.Parse(userId), commentId, replyId);
            return Ok(new { message = "Comentário deletado com sucesso!" });
        }
        [HttpPost("Like")]
        [Authorize(Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            await this.addLike.ExecuteAsync(Guid.Parse(userId), likeRequest);
            return Ok();
        }
        [HttpDelete("Like")]
        [Authorize(Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> RemoveLike([FromBody] LikeRequest likeRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            await this.removeLike.ExecuteAsync(Guid.Parse(userId), likeRequest);
            return Ok();
        }
    }
}