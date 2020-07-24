using System;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.DTOs.Picture
{
	public class PictureCreateDTO : TrackableModify
	{
		public string PictureName { get; set; }

		public string PicturePath { get; set; }

		public Guid? PostId { get; set; }

		public Guid? UserId { get; set; }
	}
}
