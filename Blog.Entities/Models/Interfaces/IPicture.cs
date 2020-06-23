using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models.Interfaces
{
	interface IPicture
	{
		[Key]
		Guid Id { get; set; }

		string PictureName { get; set; }

		string PicturePath { get; set; }
	}
}
