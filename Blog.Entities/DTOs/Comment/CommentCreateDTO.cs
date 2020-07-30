using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Post;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Blog.Entities.Enums;

namespace Blog.Entities.DTOs.Comment
{
	public class CommentCreateDTO
	{
		[MaxLength(300, ErrorMessage = "Comment length can not be more than 300 characters!")]
		public string Text { get; set; }

		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		public Guid? ReplyToCommentId { get; set; }

		public CommentStatus CommentStatus { get; set; }

		public PostViewDTO PostViewDTO { get; set; }

		public DateTime Created { get; set; }

		public DateTime Modified { get; set; }

	}
}
