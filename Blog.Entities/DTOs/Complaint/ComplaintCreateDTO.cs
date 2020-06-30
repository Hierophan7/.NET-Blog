using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Comment;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Complaint
{
	public class ComplaintCreateDTO
	{
		[Required]
		public string ComplaintText { get; set; }

		public Guid UserId { get; set; }

		public Guid CommentId { get; set; }

		public CommentViewDTO CommentViewDTO { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
