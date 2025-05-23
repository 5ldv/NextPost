using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException() : base("User cannot be null")
        {
        }
    }
}
