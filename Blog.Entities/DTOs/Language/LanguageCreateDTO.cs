using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Language
{
	public class LanguageCreateDTO
	{
		[Required]
		public string LanguageName { get; set; }
	}
}
