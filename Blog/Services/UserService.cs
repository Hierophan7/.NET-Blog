using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services
{
	public class UserService : BaseService<User>, IUserService
	{
		private Repository<User> _repository;

		public UserService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<User>(blogContext);
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return (await _repository.FindByAsync(null, u => u.Picture, u => u.Posts));
		}

		public override async Task<User> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByAsync(u => u.Id == id, u => u.Picture)).FirstOrDefault();
		}

		public async Task<IEnumerable<User>> GetUsersForNitificationAsync()
		{
			return (await _repository.FindByConditionAsync(u => u.AutomaticEmailNotification == true));
		}
	}
}
