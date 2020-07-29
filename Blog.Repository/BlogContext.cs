using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Blog.Entities.Models;
using Blog.Entities.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
	public class BlogContext : IdentityDbContext<User, AppRole, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BlogContext(DbContextOptions<BlogContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
			Database.EnsureCreated();
		}

		public DbSet<Tag> Tags { get; set; }
		public DbSet<Reaction> Reations { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<TagPost> TagPosts{ get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RoleClaim>().HasKey(p => new { p.Id });
			modelBuilder.Entity<UserLogin>().HasKey(p => new { p.UserId });
			modelBuilder.Entity<UserClaim>().HasKey(p => new { p.Id });
			modelBuilder.Entity<UserRole>().HasKey(p => new { p.UserId, p.RoleId });
			modelBuilder.Entity<UserToken>().HasKey(p => new { p.UserId });
			modelBuilder.Entity<AppRole>().HasKey(p => new { p.Id });

			modelBuilder.Entity<TagPost>()
				.HasKey(bc => new { bc.TagId, bc.PostId });

			modelBuilder.Entity<TagPost>()
				.HasOne(bc => bc.Post)
				.WithMany(b => b.TagPosts)
				.HasForeignKey(bc => bc.PostId);

			modelBuilder.Entity<TagPost>()
				.HasOne(bc => bc.Tag)
				.WithMany(c => c.TagPosts)
				.HasForeignKey(bc => bc.TagId);

			base.OnModelCreating(modelBuilder);

		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			OnBeforeSaving();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			OnBeforeSaving();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		private void OnBeforeSaving()
		{
			var user = GetCurrentUserName();
			var role = GetCurrentUserRole();
			var entries = ChangeTracker.Entries();
			foreach (var entry in entries)
			{
				if (entry.Entity is ITrackable trackable)
				{
					switch (entry.State)
					{
						case EntityState.Modified:
							trackable.Created = DateTime.UtcNow;
							trackable.CreatedByName = user;
							trackable.CreatedByRole = role;
							break;

						case EntityState.Added:
							trackable.Created = DateTime.UtcNow;
							trackable.CreatedByName = user;
							trackable.CreatedByRole = role;
							break;
					}
				}
				else if (entry.Entity is ITrackableModify trackableModify)
				{
					switch (entry.State)
					{
						case EntityState.Modified:
							trackableModify.Modified = DateTime.UtcNow;
							trackableModify.ModifiedBy = user;
							break;

						case EntityState.Added:
							trackableModify.Created = DateTime.UtcNow;
							trackableModify.CreatedBy = user;
							break;
					}
				}
			}
		}

		private string GetCurrentUserName()
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext != null)
				return httpContext.User.Identity.Name;
			else
				return null;
		}

		private string GetCurrentUserRole()
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext != null)
				return httpContext.User.Claims?.Where(x => x.Type == ClaimTypes.Role)?.FirstOrDefault()?.Value;
			else
				return null;
		}
	}
}
