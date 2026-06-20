using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Domain.Enums;

namespace PostService.Application.DTOs.Request
{
    public class PostRequest : BaseRequest
    {
        public List<PostContentRequest> PostContents { get; set; }
        public List<ToolRequest> Tools { get; set; }
    }
}