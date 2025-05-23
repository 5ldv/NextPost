using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class UnactiveRefreshTokenException : Exception
    {
        public UnactiveRefreshTokenException() : base("Refresh token is expired or revoked")
        {
        }

      
    }
}
