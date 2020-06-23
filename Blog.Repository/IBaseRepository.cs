using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
	public interface IBaseRepository<TEntity>
	{
		Task<IEnumerable<TEntity>> FindAllAsync(params Expression<Func<TEntity, object>>[] includes);
		IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includes);
		Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
		void Create(TEntity entity);
		void Update(TEntity entity);
		bool Delete(TEntity entity);
		Task SaveAsync();
	}
}
