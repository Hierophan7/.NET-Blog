using System.Threading.Tasks;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Repository
{
	public class DbInitializer
	{
		public const string superAdminEmail = "travelblog1.no.reply@gmail.com";

		public const string superAdminPassword = "TravelBlog@111";

		public const string superAdminName = "supperAdmin";

		public async Task InitializeAsync(RoleManager<AppRole> roleManager, UserManager<User> userManager)
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

			var supperAdmin = new User()
			{
				Email = superAdminEmail,
				UserName = superAdminName,
			};

			IdentityResult result = await userManager.CreateAsync(supperAdmin, superAdminPassword);

			if (result.Succeeded)
				await userManager.AddToRoleAsync(supperAdmin, "SuperAdmin");
		}
	}
}
