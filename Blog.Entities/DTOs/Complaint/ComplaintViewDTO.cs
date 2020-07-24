using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Account;
using Blog.Entities.DTOs.Comment;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Complaint
{
	public class ComplaintViewDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string Text { get; set; }

		public Guid UserId { get; set; }

		public UserViewDto UserViewDto { get; set; }

		public Guid CommentId { get; set; }

		public CommentViewDTO CommentViewDTO { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
