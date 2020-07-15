using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class PictureService : BaseService<Picture>, IPictureService
	{
		private Repository<Picture> _repository;

		public PictureService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Picture>(blogContext);
		}

		public async Task<Picture> GetAvatarAsync(Guid userId)
		{
			return (await _repository.FindByConditionAsync(a => a.UserId == userId)).FirstOrDefault();
		}

		public async override Task<Picture> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(p => p.Id == id)).FirstOrDefault();
		}


	}
}
