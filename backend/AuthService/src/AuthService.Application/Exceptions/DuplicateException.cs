using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Application.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException() : base()
        {
            
        }
    }
}