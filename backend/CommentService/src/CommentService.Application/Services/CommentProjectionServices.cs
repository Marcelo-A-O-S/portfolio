using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;

namespace CommentService.Application.Services
{
    public class CommentProjectionServices : ICommentProjectionServices
    {
        private readonly ICommentProjectionRepository repository;
        public CommentProjectionServices(
            ICommentProjectionRepository _repository
        )
        {
            this.repository = _repository;
        }
        public async Task Delete(CommentProjection entity)
        {
            await this.repository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.repository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.repository.Exists(Id);
        }

        public async Task<CommentProjection> FindBy(Expression<Func<CommentProjection, bool>> predicate)
        {
            return await this.repository.FindBy(predicate);
        }

        public async Task<CommentProjection> GetById(Guid Id)
        {
            return await this.repository.GetById(Id);
        }

        public async Task<List<CommentProjection>> List()
        {
            return await this.repository.List();
        }

        public async Task<List<CommentProjection>> List(int page)
        {
            return await this.repository.List(page);
        }

        public async Task Save(CommentProjection entity)
        {
            await this.repository.Save(entity);
        }

        public async Task Update(CommentProjection entity)
        {
            await this.repository.Update(entity);
        }
    }
}