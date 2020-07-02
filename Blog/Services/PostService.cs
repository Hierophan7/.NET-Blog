using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class PostService : BaseService<Post>, IPostService
	{
		private Repository<Post> _repository;

	public PostService(BlogContext blogContext)
		: base(blogContext)
	{
		_repository = new Repository<Post>(blogContext);
	}

		public async override Task<Post> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(r => r.Id == id)).FirstOrDefault();
		}
	}
}
