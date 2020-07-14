using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Account
{
	public class UserRegisterDto
	{
		[Required(ErrorMessage = "The field is required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password mismatch")]
		public string PasswordConfirm { get; set; }
	}
}
