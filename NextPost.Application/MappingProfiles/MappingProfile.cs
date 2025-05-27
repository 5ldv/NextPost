using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NextPost.Application.DTO_s;
using NextPost.Application.Dtos;
using NextPost.Core.Models;
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

            CreateMap<AppUser, UserDto>();

            CreateMap<UpdateAuthorDto, Author>();
            CreateMap<Author, AuthorDto>();
            CreateMap<Author, PostAuthorDto>();
            CreateMap<Author, CommentAuthorDto>();

            CreateMap<Post, PostDto>();
            CreateMap<AddPostDto, Post>();

            CreateMap<Comment, PostCommentDto>();
            CreateMap<AddCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

        }

    }
}
