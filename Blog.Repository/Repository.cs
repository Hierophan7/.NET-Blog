using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Repository
{
	public class Repository<TEntity> : BaseRepository<TEntity> where TEntity : class
	{
		public Repository(BlogContext blogContext)
			: base(blogContext)
		{

		}
	}
}
