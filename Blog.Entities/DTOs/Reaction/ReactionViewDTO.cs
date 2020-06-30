using System;
using System.Text;
using System.Collections.Generic;
using Blog.Entities.DTOs.Post;

namespace Blog.Entities.DTOs.Reaction
{
	public class ReactionViewDTO
	{
		public Guid Id { get; set; }

		public bool? Like { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public Guid PostId { get; set; }

		//public PostViewDTO PostViewDTO { get; set; }

		public string UserIP { get; set; }
	}
}
