using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class CommentService : BaseService<Comment>, ICommentService
	{
		private Repository<Comment> _repository;

		public CommentService(BlogContext blogContext) 
			: base(blogContext)
		{
			_repository = new Repository<Comment>(blogContext);
		}

		public async override Task<Comment> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(c => c.Id == id)).FirstOrDefault();
		}

		//public async Task<IEnumerable<Comment>> GetCommentsForPost(Guid id)
		//{
		//	return (await _repository.FindByAsync(c => c.PostId == id, u=>u.User));
		//}
	}
}
