using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface ICategoryService : IBaseService<Category>
	{
		Task<IEnumerable<string>> GetByPrefixAsync(string Prefix);
	}
}
