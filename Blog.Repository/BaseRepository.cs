using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{
		protected DbContext EntitiesContext { get; set; }

		public BaseRepository(DbContext entitiesContext)
		{
			this.EntitiesContext = entitiesContext;
		}

		public void Create(TEntity entity)
		{
			this.EntitiesContext.Set<TEntity>().Add(entity);
		}

		public bool Delete(TEntity entity)
		{
			this.EntitiesContext.Set<TEntity>().Remove(entity);
			return true;
		}

		public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
		{
			return this.EntitiesContext.Set<TEntity>().AsQueryable<TEntity>();
		}

		public async Task UpdateEntryAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
		{
			var entry = EntitiesContext.Entry(entity);

			EntitiesContext.Set<TEntity>().Attach(entity);

			foreach (var property in properties)
				entry.Property(property).IsModified = true;

			await EntitiesContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<TEntity>> FindAllAsync()
		{
			return await this.EntitiesContext.Set<TEntity>().ToListAsync();
		}

		public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
		{
			return await this.EntitiesContext.Set<TEntity>().Where(expression).AsNoTracking().ToListAsync();
		}

		public async Task SaveAsync()
		{
			await this.EntitiesContext.SaveChangesAsync();
		}

		public void Update(TEntity entity)
		{
			this.EntitiesContext.Set<TEntity>().Update(entity);
		}
	}
}
