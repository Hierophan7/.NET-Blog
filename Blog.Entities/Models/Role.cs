using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
	public class Role
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string RoleName { get; set; }

		public List<User> Users { get; set; }
	}
}
