using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class UpdateUserFailedException : Exception
    {
        public UpdateUserFailedException() : base("Failed to update user")
        {

        }
    }
}
