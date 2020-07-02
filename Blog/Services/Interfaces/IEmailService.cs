using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
	public interface IEmailService
	{
		Task SendAsync(string subject, string message, string email);
	}
}
