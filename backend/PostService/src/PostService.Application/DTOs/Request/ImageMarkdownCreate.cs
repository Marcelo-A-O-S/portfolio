using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class ImageMarkdownCreate
    {
        [Required]
        public IFormFile file { get; set; }
    }
}