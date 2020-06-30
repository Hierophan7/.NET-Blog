using System;
using System.Collections.Generic;
using System.Text;
using Blog.Entities.Enums;
using Blog.Entities.DTOs.Picture;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Post
{
	public class PostUpdateDTO
	{
		public Guid Id { get; set; }

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

		public DateTime ModifiedDate { get; set; }

		public List<PictureCreateDTO> Pictures { get; set; }

		//Comments?, tags;
	}
}
