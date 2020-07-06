using System.Threading.Tasks;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Repository
{
	public class DbInitializer
	{
		public async Task InitializeAsync(RoleManager<AppRole> roleManager)
		{
			if (await roleManager.FindByNameAsync("Admin") == null)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "Admin" });
			}
			if (await roleManager.FindByNameAsync("User") == null)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "User" });
			}
			if (await roleManager.FindByNameAsync("SuperAdmin") == null)
			{
				await roleManager.CreateAsync(new AppRole() { Name = "SuperAdmin" });
			}
		}
	}
}
