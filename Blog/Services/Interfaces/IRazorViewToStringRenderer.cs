using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
	public interface IRazorViewToStringRenderer
	{
		Task<string> RenderToStringAsync(string viewName, object model);
	}
}
