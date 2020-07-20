using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Enums;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;

namespace Blog.Services
{
	public class PostService : BaseService<Post>, IPostService
	{
		private Repository<Post> _repository;

		public PostService(BlogContext blogContext)
			: base(blogContext)
		{
			_repository = new Repository<Post>(blogContext);
		}

		public async Task<IEnumerable<Post>> GetAllArchivedPostsAsync()
		{
			return (await _repository.FindByConditionAsync(s => s.PostStatus == PostStatus.Archive))
				.OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllDraftsAsync()
		{
			return (await _repository.FindByConditionAsync(s => s.PostStatus == PostStatus.Draft)).OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllPostedPostsAsync()
		{
			var posts = (await _repository.FindByConditionAsync(s => s.PostStatus == PostStatus.Posted))
				.OrderByDescending(p => p.CreationData);
			return posts;
		}

		public async Task<IEnumerable<Post>> GetAllUserArchivedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Archive && s.UserId == userID))
				.OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllUserDraftsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Draft && s.UserId == userID))
				.OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllUserPostedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Posted && s.UserId == userID))
				.OrderByDescending(p => p.CreationData);
		}

		public async override Task<Post> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByConditionAsync(r => r.Id == id)).FirstOrDefault();
		}
	}
}
