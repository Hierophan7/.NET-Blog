using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class TagService : BaseService<Tag>, ITagService
	{
		private Repository<Tag> _repository;

		public TagService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Tag>(blogContext);
		}

		public async override Task<Tag> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(t => t.Id == id)).FirstOrDefault();
		}
	}
}
