using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repository;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
	public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
	{
		private Repository<TEntity> _repository;

		public BaseService(BlogContext blogContext)
		{
			_repository = new Repository<TEntity>(blogContext);
		}

		public async Task<TEntity> CreateAsync(TEntity createObject)
		{
			_repository.Create(createObject);
			await _repository.SaveAsync();

			return createObject;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var item = await this.GetByIdAsync(id);
			if (item != null)
			{
				_repository.Delete(item);
				await _repository.SaveAsync();
				return true;
			}
			return false;
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _repository.FindAllAsync();
		}

		public abstract Task<TEntity> GetByIdAsync(Guid id);

		public async Task UpdateAsync(TEntity updateObject)
		{
			_repository.Update(updateObject);
			await _repository.SaveAsync();
		}
	}
}
