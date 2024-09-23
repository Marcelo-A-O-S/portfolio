using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_portifolio.Models
{
    public class Category
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string CategoryName { get; set; }
    }
}