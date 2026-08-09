using PostService.Application.Caching.Tools;
using PostService.Application.Caching.User;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Caching.Post;
using PostService.Application.Constants;
namespace PostService.Application.Validators
{
    public class ToolValidationServices : IToolValidationServices
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        private readonly IToolCacheServices toolCacheServices;
        private readonly IToolsServices toolsServices;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostServices postServices;
        public ToolValidationServices(
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient,
            IToolCacheServices _toolCacheServices,
            IToolsServices _toolsServices,
            IPostCacheServices _postCacheServices,
            IPostServices _postServices
        )
        {
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
            this.toolCacheServices = _toolCacheServices;
            this.toolsServices = _toolsServices;
            this.postCacheServices = _postCacheServices;
            this.postServices = _postServices;
        }

        public async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache(CacheKeys.PostExists(postId));
            if(postCache == null)
            {
                var exists = await this.postServices.Exists(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache(CacheKeys.PostExists(postId), postId);
            }
        }

        public async Task ValidateProviderExists(Guid userId, string providerId)
        {
            var providerCache = await this.userCacheServices.GetProviderCache(CacheKeys.ProviderExists(providerId));
            if(providerCache == null)
            {
                var exists = await this.userServicesClient.ProviderExistsAsync(userId, providerId);
                if (!exists)
                    throw new NotFoundException("Provider não encontrado");
                await this.userCacheServices.AddProviderCache(CacheKeys.ProviderExists(providerId), providerId);
            }
        }

        public async Task ValidateToolExists(Guid toolId)
        {
            var toolCache = await this.toolCacheServices.GetToolCache(CacheKeys.ToolExists(toolId));
            if(toolCache == null)
            {
                var exists = await this.toolsServices.Exists(toolId);
                if(!exists)
                    throw new NotFoundException("Ferramenta não encontrada");
                await this.toolCacheServices.AddToolCache(CacheKeys.ToolExists(toolId), toolId);
            }
        }

        public async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache(CacheKeys.UserExists(userId));
            if (userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache(CacheKeys.UserExists(userId), userId);
            }
        }
    }
}