using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
	public interface IBaseRepository<TEntity>
	{
		Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
		IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> match);
		Task<IEnumerable<TEntity>> FindAllAsync();
		Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
		void Create(TEntity entity);
		void Update(TEntity entity);
		bool Delete(TEntity entity);
		Task SaveAsync();
		Task UpdateEntryAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
	}
}
 