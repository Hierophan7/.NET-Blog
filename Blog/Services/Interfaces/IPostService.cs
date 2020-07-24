using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Entities.Models;

namespace Blog.Services.Interfaces
{
	public interface IPostService : IBaseService<Post>
	{
		// Get all posted posts for main page of blog
		Task<IEnumerable<Post>> GetAllPostedPostsAsync();

		// Get all drafts for role SuperAdmin 
		Task<IEnumerable<Post>> GetAllDraftsAsync();

		// Get all archived posts for role SuperAdmin
		Task<IEnumerable<Post>> GetAllArchivedPostsAsync();

		// Get all user posted posts
		Task<IEnumerable<Post>> GetAllUserPostedPostsAsync(Guid userID);
		
		// Get all user drafts
		Task<IEnumerable<Post>> GetAllUserDraftsAsync(Guid userID);
		
		// Get all user archived posts
		Task<IEnumerable<Post>> GetAllUserArchivedPostsAsync(Guid userID);

		Task<IEnumerable<Post>> SearchAsync(string searchString);

		Task UpdateEntryAsync(Post post);
	}
}
