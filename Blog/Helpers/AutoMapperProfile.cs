using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abbott.Entities.Dtos.Account;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;
using AutoMapper;


namespace Blog.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<UserRegisterDto, User>();
			CreateMap<UserUpdateDto, User>();
			CreateMap<User, UserViewDto>();
			CreateMap<UserViewDto, User>();
		}
	}
}
