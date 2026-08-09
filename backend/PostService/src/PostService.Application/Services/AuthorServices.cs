using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class AuthorServices : IAuthorServices
    {
        private readonly IAuthorRepository authorRepository;
        public AuthorServices(
            IAuthorRepository _authorRepository
        )
        {
            this.authorRepository = _authorRepository;
        }
        public async Task Delete(Author entity)
        {
            await this.authorRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.authorRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.authorRepository.Exists(Id);
        }

        public async Task<Author> FindBy(Expression<Func<Author, bool>> predicate)
        {
            return await this.authorRepository.FindBy(predicate);
        }

        public async Task<Author> GetById(Guid Id)
        {
            return await this.authorRepository.GetById(Id);
        }

        public async Task<List<Author>> List()
        {
            return await this.authorRepository.List();
        }

        public async Task<List<Author>> List(int page)
        {
            return await this.authorRepository.List(page);
        }

        public async Task Save(Author entity)
        {
            await this.authorRepository.Save(entity);
        }

        public async Task Update(Author entity)
        {
            await this.authorRepository.Update(entity);
        }
    }
}