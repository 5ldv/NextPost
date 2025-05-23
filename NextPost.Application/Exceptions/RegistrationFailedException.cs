using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException(IEnumerable<string> errors) : 
            base($"Registration failed: {string.Join("; ", errors)}")
        {
        }
    }
}
