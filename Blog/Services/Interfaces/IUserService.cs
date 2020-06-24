using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	interface IUserService : IBaseService<User>
	{
		Task<User> CreateAsync(User user, string password);
		Task<bool> ChangePassword(Guid userId, string newPassword);
		Task<bool> ChangePassword(Guid userId, string newPassword, string oldPassword);
	}
}
