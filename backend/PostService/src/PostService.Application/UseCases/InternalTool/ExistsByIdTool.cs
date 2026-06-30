using PostService.Application.Interfaces;
using PostService.Application.UseCases.InternalTool.Interfaces;

namespace PostService.Application.UseCases.InternalTool
{
    public class ExistsByIdTool : IExistsByIdTool
    {
        private readonly IToolsServices toolsServices;
        public ExistsByIdTool(
            IToolsServices _toolsServices
        )
        {
            this.toolsServices = _toolsServices;
        }
        public async Task<bool> ExecuteAsync(Guid Id)
        {
            return await this.toolsServices.Exists(Id);
        }
    }
}