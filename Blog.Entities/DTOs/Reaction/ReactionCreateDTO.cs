using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities.DTOs.Reaction
{
	public class ReactionCreateDTO
	{
		public bool? Like { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public Guid PostId { get; set; }

		public string UserIP { get; set; }
	}
}
