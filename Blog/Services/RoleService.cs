using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	

	public class RoleService : BaseService<Role>, IRoleService
	{
		private Repository<Role> _repository;

		public RoleService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Role>(blogContext);
		}

		public override async Task<Role> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(r => r.Id == id)).FirstOrDefault();
		}

		public async Task<Role> GetRoleByName(string roleName)
		{
			return (await _repository.FindByConditionAsync(r => r.RoleName == roleName)).FirstOrDefault();
		}
	}
}
