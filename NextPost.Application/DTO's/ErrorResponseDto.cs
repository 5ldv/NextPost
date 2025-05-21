using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.Dtos
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public required string Title { get; set; }
        public required List<string> ExceptionMessages { get; set; }

    }
}
