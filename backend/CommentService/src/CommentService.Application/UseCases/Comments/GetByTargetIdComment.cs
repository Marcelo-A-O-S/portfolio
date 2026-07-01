using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Interfaces;
using CommentService.Application.DTOs.Request;

namespace CommentService.Application.UseCases.Comments
{
    public class GetByTargetIdComment : IGetByTargetIdComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        public GetByTargetIdComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
        }
        public async Task ExecuteAsync(GetCommentRequest request)
        {
            var comment = await this.commentServices.FindBy(c => c.TargetId == request.TargetId && c.Type == request.Type);
            throw new NotImplementedException();
        }
    }
}