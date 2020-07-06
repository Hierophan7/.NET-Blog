namespace Blog.Common
{
	public static class PasswordRecoverySettings
	{
		public const string subject = "Відновлення паролю";

		public static string GetMessageWihtFeedBack(string feedBack)
		{
			var message = $"Для відновлення паролю, перейдіть за посиланням <a href='{feedBack}'>Travel blog</a>";

			return message;
		}
	}
}
