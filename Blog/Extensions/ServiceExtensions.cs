using Blog.Services;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureServices(this IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddScoped<IPictureService, PictureService>();
			services.AddScoped<IReactionService, ReactionService>();
			services.AddScoped<ITagService, TagService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<ILanguageService, LanguageService>();
			services.AddScoped<IAutomaticEmailNotificationService, AutomaticEmailNotificationService>();
			services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
		}
	}
}
