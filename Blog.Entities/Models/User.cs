﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.Models
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }

		[Display(Name = "Хеш пароля")]
		public byte[] PasswordHash { get; set; }

		[Display(Name = "Соль хеша пароля")]
		public byte[] PasswordSalt { get; set; }

		public string InstagramLink { get; set; }

		public string YoutubeLink { get; set; }

		public string FacebookLink { get; set; }

		public string LinkedInLink { get; set; }

		public string TwitterLink { get; set; }

		public Guid RoleId { get; set; }

		public Role Role { get; set; }

		public Picture Picture { get; set; }

		public List<Post> Posts { get; set; }

		public List<Comment> Comments { get; set; }

		public List<Complaint> Complaints { get; set; }
	}
}