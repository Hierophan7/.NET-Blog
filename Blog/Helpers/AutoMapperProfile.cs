using Abbott.Entities.Dtos.Account;
using AutoMapper;
using Blog.Entities.DTOs;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Category;
using Blog.Entities.DTOs.Comment;
using Blog.Entities.DTOs.Complaint;
using Blog.Entities.DTOs.Language;
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
			CreateMap<User, UserUpdateDto>();
			CreateMap<User, UserViewDto>();
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

			CreateMap<ReactionCreateDTO, Reaction>();
			CreateMap<Reaction, ReactionViewDTO>();

			CreateMap<TagCreateDTO, Tag>();
			CreateMap<TagUpdateDTO, Tag>();
			CreateMap<Tag, TagViewDTO>();

			CreateMap<Language, LanguageViewDTO>();
			CreateMap<LanguageCreateDTO, Language>();
			
			CreateMap<PostCreateDTO, Post>();
			CreateMap<PostUpdateDTO, Post>();
				//.ForMember(c => c.Category, o => o.MapFrom(s => s.CategoryViewDTO))
				//.ForMember(c => c.User, o => o.MapFrom(s => s.UserViewDto))
				//.ForMember(c => c.Language, o => o.MapFrom(s => s.LanguageViewDTO))
				//.ForMember(d => d.Pictures, o => o.MapFrom(s => s.PictureViewDTOs));
			CreateMap<Post, PostUpdateDTO>()
				.ForMember(c => c.CategoryViewDTO, o => o.MapFrom(s => s.Category))
				.ForMember(c => c.UserViewDto, o => o.MapFrom(s => s.User))
				.ForMember(c => c.LanguageViewDTO, o => o.MapFrom(s => s.Language))
				.ForMember(d => d.PictureViewDTOs, o => o.MapFrom(s => s.Pictures));
			CreateMap<Post, PostViewDTO>()
				.ForMember(d => d.CategoryViewDTO, o => o.MapFrom(s => s.Category))
				.ForMember(d => d.UserViewDto, o => o.MapFrom(s => s.User))
				.ForMember(d => d.LanguageViewDTO, o => o.MapFrom(s => s.Language))
				.ForMember(d=>d.PictureViewDTOs, o=>o.MapFrom(s=>s.Pictures));
		}
	}
}
