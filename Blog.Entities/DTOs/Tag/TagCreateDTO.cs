using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Entities.DTOs.Tag
{
	public class TagCreateDTO
	{
		[Required]
		public string TagName { get; set; }
	}
}
