using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Application.UseCases.Languages.Interfaces
{
    public interface IDeleteLanguage
    {
        Task ExecuteAsync(Guid Id);
    }
}