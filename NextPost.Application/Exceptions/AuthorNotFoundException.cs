using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Exceptions
{
    public class AuthorNotFoundException : Exception
    {
        public AuthorNotFoundException(int authorId) : base($"Author with id ({authorId}) not found") { }
        public AuthorNotFoundException() : base($"Author not found") { }
    }
}
