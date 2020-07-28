using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Comment : TrackableModify
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Text { get; set; }

		public bool PositiveComment { get; set; }

		[ForeignKey("UserId")]
		public Guid? UserId { get; set; }

		public User User { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }

		public List<Complaint> Complaints { get; set; }
	}
}
