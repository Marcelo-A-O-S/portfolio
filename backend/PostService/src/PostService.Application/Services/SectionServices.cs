using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class SectionServices : ISectionServices
    {
        private readonly ISectionRepository sectionRepository;
        public SectionServices(ISectionRepository _sectionRepository)
        {
            this.sectionRepository = _sectionRepository;
        }
        public async Task Delete(Section entity)
        {
            await this.sectionRepository.Delete(entity);
        }

        public async Task<Section> FindBy(Expression<Func<Section, bool>> predicate)
        {
            return await this.sectionRepository.FindBy(predicate);
        }

        public async Task<Section> GetById(Guid Id)
        {
            return await this.sectionRepository.GetById(Id);
        }

        public async Task<List<Section>> List()
        {
            return await this.sectionRepository.List();
        }

        public async Task<List<Section>> List(int page)
        {
            return await this.sectionRepository.List(page);
        }

        public async Task Save(Section entity)
        {
            await this.sectionRepository.Save(entity);
        }

        public async Task Update(Section entity)
        {
            await this.sectionRepository.Update(entity);
        }
    }
}