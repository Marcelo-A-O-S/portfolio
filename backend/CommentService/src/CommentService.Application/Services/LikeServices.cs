using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
namespace CommentService.Application.Services
{
    public class LikeServices : ILikeServices
    {
        private readonly ILikeRepository likeRepository;
        public LikeServices(ILikeRepository _likeRepository)
        {
            this.likeRepository = _likeRepository;
        }
        public async Task Delete(Like entity)
        {
            await this.likeRepository.Delete(entity);
        }

        public async Task<Like> FindBy(Expression<Func<Like, bool>> predicate)
        {
            return await this.likeRepository.FindBy(predicate);
        }

        public async Task<Like> GetById(Guid Id)
        {
            return await this.likeRepository.GetById(Id);
        }

        public async Task<List<Like>> List()
        {
            return await this.likeRepository.List();
        }

        public async Task<List<Like>> List(int page)
        {
            return await this.likeRepository.List(page);
        }

        public async Task Save(Like entity)
        {
            await this.likeRepository.Save(entity);
        }

        public async Task Update(Like entity)
        {
            await this.likeRepository.Update(entity);
        }
    }
}