using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Account
{
	public class UserRegisterDto
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Email { get; set; }

		[Required]
		public int RoleId { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
