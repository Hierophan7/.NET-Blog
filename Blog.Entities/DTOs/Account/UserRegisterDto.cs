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
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password mismatch")]
		public string PasswordConfirm { get; set; }
	}
}
