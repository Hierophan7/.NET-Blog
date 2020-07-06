using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Account;

namespace Blog.Common
{
	public static class ChangePasswordSettings
	{
		public const string subject = "Відновлення паролю";

		public static string GetMessage(string email, string password)
		{
			var message = $"Ваш пароль був успішно змінений. Ваші нові дані для входу на сайт:<br />" +
				$"Email: {email} <br />" +
				$"Пароль: {password}";

			return message;
		}
	}
}
