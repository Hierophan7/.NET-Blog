using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class LanguageService : BaseService<Language>, ILanguageService
	{
		private Repository<Language> _repository;

		public LanguageService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Language>(blogContext);
		}

		public override async Task<Language> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(i => i.Id == id)).FirstOrDefault();
		}
	}
}
