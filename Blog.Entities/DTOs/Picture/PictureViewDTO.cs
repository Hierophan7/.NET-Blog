using System;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.DTOs.Picture
{
	public class PictureViewDTO : TrackableModify
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }

		public Guid? PostId { get; set; }

		public Guid? UserId { get; set; }

		public bool Delete { get; set; }

	}
}
