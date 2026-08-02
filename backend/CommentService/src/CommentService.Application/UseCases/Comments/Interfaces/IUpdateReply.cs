using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IUpdateReply
    {
        /// <summary>
        /// Caso de uso responsável por atualizar a resposta do comentário ou resposta relacionada
        /// </summary>
        /// <param name="authenticatedUserId">Identificador do usuário</param>
        /// <param name="role">Função relacionada ao usuário</param>
        /// <param name="commentId">Identificador do comentário ou resposta relacionada</param>
        /// <param name="replyId">Identificador da resposta que será atualizada</param>
        /// <param name="commentRequest">Corpo de requisição</param>
        Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId, Guid replyId, CommentRequest commentRequest);
    }
}