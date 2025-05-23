using NextPost.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.DTO_s
{
    public class PostAuthorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
