using System;
using System.Collections.Generic;
using System.Text;
using Blog.Entities.Enums;
using Blog.Entities.Models;
using Blog.Entities.DTOs.Picture;
using Blog.Entities.DTOs.Comment;
using Blog.Entities.DTOs.Category;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Post
{
	public class PostCreateDTO
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Text { get; set; }

		[Required]
		public PostStatus PostStatus { get; set; }

		public bool CommentingPermission { get; set; }

		public Guid CategoryId { get; set; }

		public Guid UserId { get; set; }

		public Guid LanguageId { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public List<CommentCreateDTO> Comments { get; set; }
		
		public List<PictureCreateDTO> Pictures { get; set; }

		//Tags;

	}
}
