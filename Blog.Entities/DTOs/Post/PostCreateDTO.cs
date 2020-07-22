using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blog.Entities.DTOs.Category;
using Blog.Entities.DTOs.Picture;
using Blog.Entities.Enums;

namespace Blog.Entities.DTOs.Post
{
	public class PostCreateDTO
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Text { get; set; }

		[StringLength(300, ErrorMessage = "Description's length cann't be more than 300 characters!")]
		public string Description { get; set; }

		public PostStatus PostStatus { get; set; }

		public bool CommentingPermission { get; set; }

		public Guid CategoryId { get; set; }

		public CategoryCreateDTO CategoryCreateDTO { get; set; }

		public List<CategoryViewDTO> CategoryViewDTOs { get; set; }

		public Guid UserId { get; set; }

		public Guid LanguageId { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public List<PictureCreateDTO> Pictures { get; set; }


	}
}
