using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class ReactionService : BaseService<Reaction>, IReactionService
	{
		private Repository<Reaction> _repository;

		public ReactionService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Reaction>(blogContext);
		}

		public async override Task<Reaction> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(r => r.Id == id)).FirstOrDefault();
		}
	}
}
