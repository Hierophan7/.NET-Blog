using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Post;
using Blog.Entities.DTOs.Complaint;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Comment
{
	public class CommentUpdateDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string Text { get; set; }

		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		public PostViewDTO PostViewDTO { get; set; }

		public DateTime Modified { get; set; }

		public List<ComplaintViewDTO> Complaints { get; set; }
	}
}
