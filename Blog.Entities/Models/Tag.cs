using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
	public class Tag
	{
		[Key]
		public Guid Id { get; set; }

		public string TagName { get; set; }

		public List<TagPost> TagPosts { get; set; }
	}
}
