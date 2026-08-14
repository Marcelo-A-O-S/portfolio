using PostService.Application.DTOs.Request;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.LinkTypes.Interfaces
{
    public interface ICreateLinkType
    {
        Task ExecuteAsync(LinkTypeRequest request);
    }
}