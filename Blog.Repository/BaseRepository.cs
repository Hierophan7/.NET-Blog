using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Repository
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{


		public void Create(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public bool Delete(TEntity entity)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includes)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TEntity>> FindAllAsync(params Expression<Func<TEntity, object>>[] includes)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
		{
			throw new NotImplementedException();
		}

		public Task SaveAsync()
		{
			throw new NotImplementedException();
		}

		public void Update(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
