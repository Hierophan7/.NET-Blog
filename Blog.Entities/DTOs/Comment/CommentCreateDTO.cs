using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities.DTOs.Comment
{
	public class CommentCreateDTO
	{
		public string CommentText { get; set; }

		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		//public Post Post { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		//public List<Complaint> Complaints { get; set; }
	}
}
