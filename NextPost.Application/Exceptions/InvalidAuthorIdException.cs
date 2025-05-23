using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class InvalidAuthorIdException : Exception
    {
        public InvalidAuthorIdException(int authorId) : base($"Author id ({authorId}) is not valid") { }
    }
}
