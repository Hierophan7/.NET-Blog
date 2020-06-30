using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Entities.DTOs.Category
{
	public class CategoryCreateDTO
	{
		[Required]
		public string CategoryName { get; set; }
	}
}
