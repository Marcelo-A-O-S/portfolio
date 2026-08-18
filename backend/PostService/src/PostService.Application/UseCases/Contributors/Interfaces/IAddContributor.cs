using PostService.Application.DTOs.Request;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Contributors.Interfaces
{
    public interface IAddContributor
    {
        Task ExecuteAsync(ContributorRequest request);
    }
}