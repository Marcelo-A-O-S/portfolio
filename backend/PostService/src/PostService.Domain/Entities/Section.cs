using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Domain.Entities
{
    public class Section
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set;}
        public string? Title { get; private set;}
        public string Content { get; private set;}
        public int Order { get; private set;}
        public DateTime CreatedAt { get; private set;}
    }
}