using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface IUserService : IBaseService<User>
	{
		Task<IEnumerable<User>> GetAllUsersAsync();

		Task<IEnumerable<User>> GetUsersForNitificationAsync();
	}
}
