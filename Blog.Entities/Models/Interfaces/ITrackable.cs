using System;

namespace Blog.Entities.Models.Interfaces
{
	public interface ITrackable
	{
		DateTime Created { get; set; }
		string CreatedByName { get; set; }
		string CreatedByRole { get; set; }
	}
}
