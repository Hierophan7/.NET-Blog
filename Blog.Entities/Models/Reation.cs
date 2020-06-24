using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Blog.Entities.Models.Interfaces;

namespace Blog.Entities.Models
{
	public class Reation : IBaseEntity
	{
		[Key]
		public Guid Id { get; set; }

		public bool? Like { get; set; }

		public DateTime CreationData { get; set; }

		public DateTime ModifiedDate { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }

		public string UserIP { get; set; }

	}
}
