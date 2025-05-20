using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NextPost.Application.Dtos;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Application.MappingProfiles
{
    internal class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<RegisterDto, AppUser>();
        }

    }
}
