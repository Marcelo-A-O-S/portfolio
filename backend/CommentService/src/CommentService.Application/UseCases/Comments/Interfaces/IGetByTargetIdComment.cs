using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IGetByTargetIdComment
    {
        Task ExecuteAsync(GetCommentRequest request);
    }
}