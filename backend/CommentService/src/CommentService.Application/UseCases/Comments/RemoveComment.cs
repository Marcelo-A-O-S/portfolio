using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Domain.Entities;

namespace CommentService.Application.UseCases.Comments
{
    public class RemoveComment : IRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        public RemoveComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
        }
        public async Task ExecuteAsync(Guid commentId)
        {
            await ValidateComment(commentId);
            await this.commentServices.DeleteById(commentId);
            await this.commentCacheServices.RemoveCommentCache($"comment:{commentId}");
        }
        private async Task ValidateComment(Guid commentId)
        {
            var commentCache = await this.commentCacheServices.GetCommentCache($"comment:{commentId}");
            if(commentCache == null)
            {
                var comment = await this.commentServices.GetById(commentId);
                if(comment == null)
                    throw new NotFoundException("Comentário não encontrado");
            }
        }
        
    }
}