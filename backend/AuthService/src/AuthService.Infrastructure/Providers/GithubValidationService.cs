using AuthService.Application.DTOs.Request;
using AuthService.Application.Interfaces;
using Octokit;
namespace AuthService.Infrastructure.Providers
{
    public class GithubValidationService : IProviderValidationService
    {
        private readonly GitHubClient client;
        public GithubValidationService(
            GitHubClient client)
        {
            this.client = client;
        }
        public async Task<ProviderUserData> Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Token não informado");

            this.client.Credentials = new Credentials(token);
            try
            {
                var user = await this.client.User.Current();
                if (user == null)
                    throw new UnauthorizedAccessException("Token do Github inválido");
                var emails = await this.client.User.Email.GetAll();
                var primaryEmail = emails.FirstOrDefault(x => x.Primary && x.Verified)?.Email;
                if (string.IsNullOrWhiteSpace(primaryEmail))
                    throw new UnauthorizedAccessException("GitHub não retornou email válido");
                return new ProviderUserData
                {
                    ProviderId = user.Id.ToString(),
                    Email = primaryEmail,
                    Name = user.Name ?? user.Login,
                    Username = user.Login,
                    PictureUrl = user.AvatarUrl,
                    VerifiedAccount = true
                };
            }
            catch (AuthorizationException)
            {
                throw new UnauthorizedAccessException("Token Github inválido");
            }

        }
    }
}