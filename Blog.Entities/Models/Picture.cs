using System;
using System.ComponentModel.DataAnnotations;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Picture : TrackableModify
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
