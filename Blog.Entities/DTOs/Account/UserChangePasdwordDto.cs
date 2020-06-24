namespace Blog.Entities.DTOs.Account
{
	public class UserChangePasdwordDto : UserNewPasdwordDto
	{
		public string OldPassword { get; set; }
	}

	public class UserNewPasdwordDto
	{
		public int Id { get; set; }
		public string NewPassword { get; set; }
		public string NewPasswordConfirm { get; set; }
	}
}
