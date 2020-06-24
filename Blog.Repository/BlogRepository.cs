namespace Blog.Repository
{
	public class BlogRepository<TEntity> : BaseRepository<TEntity> where TEntity : class
	{
		public BlogRepository(BlogContext context)
			: base(context)
		{

		}
	}
}
