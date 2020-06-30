namespace Blog.Entities.DTOs.Account
{
	public class UserChangePasswordDto : UserNewPasswordDto
	{
		public string OldPassword { get; set; }
	}

	public class UserNewPasswordDto
	{
		public int Id { get; set; }
		public string NewPassword { get; set; }
		public string NewPasswordConfirm { get; set; }
	}
}
