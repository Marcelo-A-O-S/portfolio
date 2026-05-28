using CommentService.Application.DTOs.Request;
using CommentService.Application.UseCases.Comments.Interfaces;

namespace CommentService.Application.UseCases.Comments
{
    public class UpdateComment : IUpdateComment
    {
        public UpdateComment()
        {
            
        }
        public Task ExecuteAsync(Guid commentId, CommentRequest commentRequest)
        {
            throw new NotImplementedException();
        }
    }
}