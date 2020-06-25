using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities.Models
{
	public class TagPost
	{
		public Guid PostId { get; set; }
		public Post Post { get; set; }

		public Guid TagId { get; set; }
		public Tag Tag { get; set; }
	}
}
