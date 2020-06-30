using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities.DTOs.Picture
{
	public class PictureCreateDTO
	{
		//Оно нам нужно?
		public string PictureName { get; set; }

		public string PicturePath { get; set; }

		public Guid? PostId { get; set; }

		public Guid? UserId { get; set; }

	}
}
