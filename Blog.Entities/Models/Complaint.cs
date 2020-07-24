using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Complaint : IBaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Text { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public Guid? UserId { get; set; }

		public User User { get; set; }

		public Guid CommentId { get; set; }

		public Comment Comment { get; set; }
	}
}
