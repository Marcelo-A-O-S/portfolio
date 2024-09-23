using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_portifolio.Models
{
    public class Section
    {
        public long Id { get; set; }
        public Guid Guid {get; set;}
        public int Sequence {get; set;}
        public List<Text> Texts{ get; set; }
        public List<Image> Images{ get; set; }

        public Section()
        {
            this.Guid = Guid.NewGuid();
        }
        
    }
}