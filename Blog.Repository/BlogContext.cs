using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Blog.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repository
{
	public class BlogContext : DbContext
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public BlogContext(DbContextOptions<BlogContext> options, IHttpContextAccessor httpContextAccessor)
			: base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Reation> Reations { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Complaint> Complaints { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>().HasData(
				new Role[]
				{
					new Role{ Id = Guid.NewGuid(), RoleName = "Admin"},
					new Role{Id = Guid.NewGuid(), RoleName = "Superadmin"},
					new Role{Id = Guid.NewGuid(), RoleName = "User"}
				});
		}
	}
}
