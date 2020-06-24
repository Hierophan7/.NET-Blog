namespace Blog.Entities.DTOs.Account
{
	public class UserViewDto
	{
		public int Id { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public int RoleId { get; set; }

		public string RoleName { get; set; }

		public string InstagramLink { get; set; }

		public string YoutubeLink { get; set; }

		public string FacebookLink { get; set; }

		public string LinkedInLink { get; set; }

		public string TwitterLink { get; set; }
	}
}
