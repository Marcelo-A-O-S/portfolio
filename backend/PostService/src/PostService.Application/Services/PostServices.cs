using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class PostServices : IPostServices
    {
        private readonly IPostRepository postRepository;
        public PostServices(IPostRepository _postRepository)
        {
            this.postRepository = _postRepository;
        }
        public Task Delete(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task<Post> FindBy(Expression<Func<Post, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> List()
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> List(int page)
        {
            throw new NotImplementedException();
        }

        public Task Save(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(Post entity)
        {
            throw new NotImplementedException();
        }
    }
}