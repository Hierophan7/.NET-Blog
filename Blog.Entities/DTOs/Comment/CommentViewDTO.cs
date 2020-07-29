using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Post;
using Blog.Entities.Enums;

namespace Blog.Entities.DTOs.Comment
{
	public class CommentViewDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string Text { get; set; }

		public CommentStatus CommentStatus { get; set; }

		public Guid UserId { get; set; }

		public UserViewDto UserViewDto { get; set; }

		public Guid PostId { get; set; }

		public PostViewDTO PostViewDTO { get; set; }

		public DateTime Created { get; set; }

		public DateTime Modified { get; set; }

	}
}
