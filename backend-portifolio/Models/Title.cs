using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_portifolio.Models
{
    public class Title
    {
        public long Id {get; set;}
        public Guid Guid {get; set;}
        public int Sequence {get; set;}
        public string Content {get; set;}
    }
}