using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Models
{
	public class User : IdentityUser<Guid>
	{
		public bool AutomaticEmailNotification { get; set; }

		public byte[] PasswordSalt { get; set; }

		public string InstagramLink { get; set; }

		public string YoutubeLink { get; set; }

		public string FacebookLink { get; set; }

		public string LinkedInLink { get; set; }

		public string TwitterLink { get; set; }

		public Picture Picture { get; set; }

		public List<Post> Posts { get; set; }

		public List<Comment> Comments { get; set; }

		public List<Complaint> Complaints { get; set; }
	}
}
