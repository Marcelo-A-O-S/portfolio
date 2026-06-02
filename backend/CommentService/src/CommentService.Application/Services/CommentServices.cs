using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
namespace CommentService.Application.Services
{
    public class CommentServices : ICommentServices
    {
        private readonly ICommentRepository commentRepository;
        public CommentServices(ICommentRepository _commentRepository)
        {
            this.commentRepository = _commentRepository;
        }
        public async Task Delete(Comment entity)
        {
            await this.commentRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.commentRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.commentRepository.Exists(Id);
        }

        public async Task<Comment> FindBy(Expression<Func<Comment, bool>> predicate)
        {
            return await this.commentRepository.FindBy(predicate);
        }

        public async Task<Comment> GetById(Guid Id)
        {
            return await this.commentRepository.GetById(Id);
        }

        public async Task<List<Comment>> GetCommentsByPostId(Guid postId)
        {
            return await this.commentRepository.GetCommentsByPostId(postId);
        }

        public async Task<List<Comment>> List()
        {
            return await this.commentRepository.List();
        }

        public async Task<List<Comment>> List(int page)
        {
            return await this.commentRepository.List(page);
        }

        public async Task Save(Comment entity)
        {
            await this.commentRepository.Save(entity);
        }

        public async Task Update(Comment entity)
        {
            await this.commentRepository.Update(entity);
        }
    }
}