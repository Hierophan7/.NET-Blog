using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entities.DTOs.Account
{
	public class UserChangePasswordDto : UserNewPasswordDto
	{
		public string OldPassword { get; set; }
	}

	public class UserNewPasswordDto
	{
		public Guid Id { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "The field is required")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Password mismatch")]
		public string NewPasswordConfirm { get; set; }

		public string Email { get; set; }
	}
}
