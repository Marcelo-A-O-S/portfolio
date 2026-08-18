using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Contributors.Interfaces
{
    public interface IUpdateContributor
    {
        Task ExecuteAsync(Guid Id, ContributorRequest request);
    }
}