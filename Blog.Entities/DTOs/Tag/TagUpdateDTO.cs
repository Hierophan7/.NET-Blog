using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Entities.DTOs.Tag
{
	public class TagUpdateDTO
	{
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
