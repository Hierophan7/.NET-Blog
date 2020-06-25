using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Comment : IBaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string CommentText { get; set; }

		[ForeignKey("UserId")]
		public Guid? UserId { get; set; }

		public User User { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public List<Complaint> Complaints { get; set; }
	}
}
