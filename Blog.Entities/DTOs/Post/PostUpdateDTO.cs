using System;
using System.Collections.Generic;
using System.Text;
using Blog.Entities.Enums;
using Blog.Entities.DTOs.Picture;
using System.ComponentModel.DataAnnotations;
using Blog.Entities.DTOs.Category;
using System.Reflection.Metadata.Ecma335;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Language;
using Microsoft.AspNetCore.Http;

namespace Blog.Entities.DTOs.Post
{
	public class PostUpdateDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string Title { get; set; }

		[StringLength(500, ErrorMessage = "Description's length cann't be more than 500 characters!")]
		public string Description { get; set; }

		[Required]
		public string Text { get; set; }

		public PostStatus PostStatus { get; set; }

		public bool CommentingPermission { get; set; }

		public Guid CategoryId { get; set; }
		public CategoryViewDTO CategoryViewDTO { get; set; }

		public Guid UserId { get; set; }
		public UserViewDto UserViewDto { get; set; }

		public string ModifiedBy { get; set; }

		public Guid LanguageId { get; set; }
		public LanguageViewDTO LanguageViewDTO { get; set; }

		public DateTime Modified { get; set; }

		public List<PictureViewDTO> PictureViewDTOs { get; set; }

		public IFormFileCollection NewPictures { get; set; }

		//Comments?, tags;
	}
}
