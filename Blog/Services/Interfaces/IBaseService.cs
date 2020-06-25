using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
	interface IBaseService<TEntity> where TEntity : class
	{
        Task<IEnumerable<TEntity>> GetAllAsync();
        //Task<PageViewModel<TEntity>> GetAllPerPageAsync(int? page, int? pageSize);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> CreateAsync(TEntity createObject);
        Task UpdateAsync(TEntity updateObject);
        Task<bool> DeleteAsync(Guid id);
    }
}
