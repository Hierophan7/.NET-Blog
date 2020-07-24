using System;

namespace Blog.Entities.Models.Interfaces
{
	public abstract class TrackableModify : ITrackableModify
	{
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? Modified { get; set; }
		public string ModifiedBy { get; set; }
	}
}
