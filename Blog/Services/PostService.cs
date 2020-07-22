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
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Archive, i => i.Category, i => i.User, i => i.Language))
				.OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllDraftsAsync()
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Draft, i => i.Category, i => i.User, i => i.Language))
				.OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllPostedPostsAsync()
		{
			var posts = (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Posted, i => i.Category, i => i.User, i => i.Language))
				.OrderByDescending(p => p.CreationData);
			return posts;
		}

		public async Task<IEnumerable<Post>> GetAllUserArchivedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Archive && s.UserId == userID,
				i => i.Category, i => i.User, i => i.Language)).OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllUserDraftsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Draft && s.UserId == userID,
				 i => i.Category, i => i.User, i => i.Language)).OrderByDescending(p => p.CreationData);
		}

		public async Task<IEnumerable<Post>> GetAllUserPostedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Posted && s.UserId == userID,
				 i => i.Category, i => i.User, i => i.Language)).OrderByDescending(p => p.CreationData);
		}

		public async override Task<Post> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByAsync(r => r.Id == id, i => i.Category, i => i.User, i => i.Language)).FirstOrDefault();
		}
	}
}
