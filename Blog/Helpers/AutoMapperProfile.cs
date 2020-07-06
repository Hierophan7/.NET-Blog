using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Entities.DTOs;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Category;
using Blog.Entities.DTOs.Comment;
using Blog.Entities.DTOs.Complaint;
using Blog.Entities.DTOs.Picture;
using Blog.Entities.DTOs.Post;
using Blog.Entities.DTOs.Reaction;
using Blog.Entities.DTOs.Tag;
using Blog.Entities.Models;

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
			CreateMap<User, UserChangePasswordDto>();
			CreateMap<User, UserNewPasswordDto>();

			CreateMap<CategoryCreateDTO, Category>();
			CreateMap<Category, CategoryViewDTO>();

			CreateMap<CommentCreateDTO, Comment>();
			CreateMap<CommentUpdateDTO, Comment>();
			CreateMap<Comment, CommentViewDTO>();

			CreateMap<ComplaintCreateDTO, Complaint>();
			CreateMap<Complaint, ComplaintViewDTO>();
			CreateMap<ComplaintUpdateDTO, Complaint>();

			CreateMap<PictureCreateDTO, Picture>();
			CreateMap<Picture, PictureViewDTO>();

			CreateMap<PostCreateDTO, Post>();
			CreateMap<PostUpdateDTO, Post>();
			CreateMap<Post, PostViewDTO>();

			CreateMap<ReactionCreateDTO, Reaction>();
			CreateMap<Reaction, ReactionViewDTO>();

			CreateMap<TagCreateDTO, Tag>();
			CreateMap<TagUpdateDTO, Tag>();
			CreateMap<Tag, TagViewDTO>();

			CreateMap<Role, RoleViewDTO>();
		}
	}
}
