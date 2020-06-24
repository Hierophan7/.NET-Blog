using System.ComponentModel.DataAnnotations;

namespace Abbott.Entities.Dtos.Account
{
	public class UserUpdateDto
	{
		[Required, MaxLength(50)]
		public string UserName { get; set; }

		[Required, MaxLength(50)]
		public string Email { get; set; }

		[Required]
		public int RoleId { get; set; }

		public string InstagramLink { get; set; }

		public string YoutubeLink { get; set; }

		public string FacebookLink { get; set; }

		public string LinkedInLink { get; set; }

		public string TwitterLink { get; set; }

	}
}
