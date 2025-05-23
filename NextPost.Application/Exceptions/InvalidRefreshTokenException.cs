using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException(): 
            base("Invalid refresh token")
        {
        }
        public InvalidRefreshTokenException(string refreshToken) : 
            base($"Invalid refresh token: ({refreshToken})")
        {
        }
    }
}
