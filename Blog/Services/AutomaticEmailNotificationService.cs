using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public class AutomaticEmailNotificationService : IAutomaticEmailNotificationService
	{
		private readonly IEmailService _emailService;

		public AutomaticEmailNotificationService(IEmailService emailService)
		{
			_emailService = emailService;
		}

		public async Task SentAutomaticNotificationAsync(string subject, string message, List<UserViewDto> users)
		{
			foreach(var user in users)
			{
				await _emailService.SendAsync(subject, message, user.Email);
			}
		}
	}
}
