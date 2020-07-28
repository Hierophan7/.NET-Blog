using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Enums;
using Blog.Entities.Models;
using Blog.Repository;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
	public class PostService : BaseService<Post>, IPostService
	{
		private Repository<Post> _repository;
		private BlogContext _blogContext;

		public PostService(BlogContext blogContext)
			: base(blogContext)
		{
			_blogContext = blogContext;
			   _repository = new Repository<Post>(blogContext);
		}

		public async Task<IEnumerable<Post>> GetAllArchivedPostsAsync()
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Archive, i => i.Category, i => i.User, i => i.Language))
				.OrderByDescending(p => p.Created);
		}

		public async Task<IEnumerable<Post>> GetAllDraftsAsync()
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Draft, i => i.Category, i => i.User, i => i.Language,
				i => i.Pictures)).OrderByDescending(p => p.Created);
		}

		public async Task<IEnumerable<Post>> GetAllPostedPostsAsync()
		{
			var posts = (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Posted, i => i.Category, i => i.User, i => i.Language,
			i => i.Pictures)).OrderByDescending(p => p.Created);
			return posts;
		}

		public async Task<IEnumerable<Post>> GetAllUserArchivedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Archive && s.UserId == userID,
				i => i.Category, i => i.User, i => i.Language, i => i.Pictures)).OrderByDescending(p => p.Created);
		}

		public async Task<IEnumerable<Post>> GetAllUserDraftsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Draft && s.UserId == userID,
				 i => i.Category, i => i.User, i => i.Language, i => i.Pictures)).OrderByDescending(p => p.Created);
		}

		public async Task<IEnumerable<Post>> GetAllUserPostedPostsAsync(Guid userID)
		{
			return (await _repository.FindByAsync(s => s.PostStatus == PostStatus.Posted && s.UserId == userID,
				 i => i.Category, i => i.User, i => i.Language, i => i.Pictures)).OrderByDescending(p => p.Created);
		}

		public async override Task<Post> GetByIdAsync(Guid id)
		{
			return (await _repository.FindByAsync(
				r => r.Id == id,
				i => i.Category,
				i => i.User,
				i => i.Language,
				i => i.Pictures,
				i => i.Comments)).FirstOrDefault();
		}

		public Post GetByIdExtend(Guid id)
		{
			var post =  (_blogContext.Posts.Where(r => r.Id == id)
					.Include(i => i.User)
					.Include(i => i.Category)
					.Include(i => i.User)
					.Include(i => i.Language)
					.Include(i => i.Pictures)
					.Include(i => i.Comments)
					.Include(i => i.Comments).ThenInclude(i => i.User)).FirstOrDefault();
			return post;
		}

		public async Task<IEnumerable<Post>> GetPostsForUser(Guid userId)
		{
			return (await _repository.FindByAsync(u => u.UserId == userId, i => i.Category, i => i.User, i => i.Language,
				i => i.Pictures));
		}

		public async Task<IEnumerable<Post>> SearchAsync(string searchString)
		{
			return (await _repository.FindByAsync(p =>
			p.Title.Contains(searchString) || p.Text.Contains(searchString) || p.Description.Contains(searchString),
			i => i.Category, i => i.User, i => i.Language, i => i.Pictures));
		}

		public async Task UpdateEntryAsync(Post post)
		{
			await _repository.UpdateEntryAsync(post, p => p.Title, p => p.Description, p => p.Text, p => p.CommentingPermission);
		}
	}
}
