using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
	public class Category
	{
		[Key]
		public Guid Id { get; set; }

		public string CategoryName { get; set; }

		public List<Post> Posts { get; set; }
	}
}
