using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Comment : IBaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		public string CommentText { get; set; }

		public Guid UserId { get; set; }

		public User Users { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public List<Complaint> Complaints { get; set; }
	}
}
