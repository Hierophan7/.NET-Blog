using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Account
{
	public class UserAuthenticateDto
	{
		[Required(ErrorMessage = "The field is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Запомнить?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }
	}
}
