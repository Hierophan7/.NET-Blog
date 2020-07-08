using System;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var rolesManager = services.GetRequiredService<RoleManager<AppRole>>();
					var userManager = services.GetRequiredService<UserManager<User>>();

					DbInitializer dbInitializer = new DbInitializer();
					await dbInitializer.InitializeAsync(rolesManager, userManager);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.UseStartup<Startup>()
			.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Trace))
			.Build();
	}
}
