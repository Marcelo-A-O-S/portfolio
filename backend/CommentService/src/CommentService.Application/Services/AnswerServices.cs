using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;

namespace CommentService.Application.Services
{
    public class AnswerServices : IAnswerServices
    {
        private readonly IAnswerRepository answerRepository;
        public AnswerServices(IAnswerRepository _answerRepository)
        {
            this.answerRepository = _answerRepository;
        }
        public async Task Delete(Answer entity)
        {
            await this.answerRepository.Delete(entity);
        }

        public async Task<Answer> FindBy(Expression<Func<Answer, bool>> predicate)
        {
            return await this.answerRepository.FindBy(predicate);
        }

        public async Task<Answer> GetById(Guid Id)
        {
            return await this.answerRepository.GetById(Id);
        }

        public async Task<List<Answer>> List()
        {
            return await this.answerRepository.List();
        }

        public async Task<List<Answer>> List(int page)
        {
            return await this.answerRepository.List(page);
        }

        public async Task Save(Answer entity)
        {
            await this.answerRepository.Save(entity);
        }

        public async Task Update(Answer entity)
        {
            await this.answerRepository.Update(entity);
        }
    }
}