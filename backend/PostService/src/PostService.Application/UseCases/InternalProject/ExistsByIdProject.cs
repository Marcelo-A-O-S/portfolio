using PostService.Application.Interfaces;
using PostService.Application.UseCases.InternalProject.Interfaces;

namespace PostService.Application.UseCases.InternalProject
{
    public class ExistsByIdProject : IExistsByIdProject
    {
        private readonly IPostServices postServices;
        public ExistsByIdProject(
            IPostServices _postServices
        )
        {
            this.postServices = _postServices;
        }
        public async Task<bool> ExecuteAsync(Guid Id)
        {
            return await this.postServices.Exists(Id);
        }
    }
}