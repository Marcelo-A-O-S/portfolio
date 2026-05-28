using CommentService.Application.UseCases.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CommentService.Application.DTOs.Request;
using CommentService.Application.DTOs.Request;

namespace CommentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IAddComment addComment;
        public CommentController(
            IAddComment _addComment
        )
        {
            this.addComment = _addComment;
        }
        public async Task<IActionResult> AddComment(CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                await this.addComment.ExecuteAsync(commentRequest);
                return Ok();
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}