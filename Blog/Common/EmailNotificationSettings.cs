using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Post;
using Blog.Entities.Models;

namespace Blog.Common
{
	public static class EmailNotificationSettings
	{
		public const string subject = "Чудові новини для вас!";

		private static string GetPostText(string postText)
		{
			if (postText.Length > 300)
				postText = postText.Substring(0, 300);

			return postText;
		}

		public static string CteateMessage(PostViewDTO postViewDTO, string stringFrom, string link)
		{
			var message = $"Користувач {postViewDTO.UserViewDto.UserName} зробив нову публікацію! <br />" +
				$"Якщо ця публікація зацікавила вас, <a href='{link}'>повністю ви можете переглянути її у нас на сайті.</a> <br />";

			message += stringFrom;

			return message;
		}
	}
}