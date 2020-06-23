using System;
using System.Collections.Generic;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class ProfileImage : IPicture
	{
		public Guid Id { get ; set ; }

		public string PictureName { get ; set ; }

		public string PicturePath { get ; set ; }

		public Guid UserId { get; set; }
		public User User { get; set; }
	}
}
