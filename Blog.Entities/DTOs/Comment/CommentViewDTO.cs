using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Blog.Entities.DTOs.Account;

namespace Blog.Entities.DTOs.Comment
{
	public class CommentViewDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string CommentText { get; set; }

		public Guid UserId { get; set; }

		public UserViewDto UserViewDto { get; set; }

		public Guid PostId { get; set; }

		//public Post Post { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		//public List<Complaint> Complaints { get; set; }
	}
}
