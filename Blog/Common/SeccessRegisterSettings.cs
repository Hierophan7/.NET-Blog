using Blog.Entities.DTOs.Account;

namespace Blog.Common
{
	public static class SeccessRegisterSettings
	{
		public const string subject = "Ви успішно зареєстровані";

		public static string CteateMessage(UserRegisterDto userRegisterDto)
		{
			var message = $"Данні вашого нового акаунту: <br /> " +
				$"Ім'я користувача: {userRegisterDto.UserName} <br /> " +
				$"Email: {userRegisterDto.Email} <br /> " +
				$"Пароль: {userRegisterDto.Password} <br /> " +
				$"Дякуємо за те, що обрали саме нас!";

			return message;
		}
	}
}
