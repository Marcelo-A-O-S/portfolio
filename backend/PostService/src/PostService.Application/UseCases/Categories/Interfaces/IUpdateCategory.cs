using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.Categories.Interfaces
{
    public interface IUpdateCategory
    {
        Task ExecuteAsync(Guid Id, CategoryRequest categoryRequest);
    }
}