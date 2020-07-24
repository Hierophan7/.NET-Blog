using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Post;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Language
{
	public class LanguageViewDTO
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string Name { get; set; }

		//public List<PostViewDTO> Posts { get; set; }
	}
}
