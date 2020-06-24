using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Entities.Models
{
	public class Picture
	{
		[Key]
		public Guid Id { get; set; }

		public string PictureName { get; set; }

		public string PicturePath { get; set; }

		public Guid? PostId { get; set; }

		public Post Post { get; set; }

		public Guid? UserId { get; set; }

		public User User { get; set; }
	}
}
