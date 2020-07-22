using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Entities.DTOs.Account;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface IAutomaticEmailNotificationService
	{
		Task SentAutomaticNotificationAsync(string subject, string message, List<UserViewDto> users);
	}
}
