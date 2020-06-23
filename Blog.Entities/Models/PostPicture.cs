using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class PostPicture : IPicture
	{
		[Key]
		public Guid Id { get; set; }

		public string PictureName { get; set; }

		public string PicturePath { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }
	}
}
