namespace Diagram
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Model1 : DbContext
	{
		public Model1()
			: base("name=ClassDiagram")
		{
		}

		public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<Comment> Comments { get; set; }
		public virtual DbSet<Complaint> Complaints { get; set; }
		public virtual DbSet<Language> Languages { get; set; }
		public virtual DbSet<Picture> Pictures { get; set; }
		public virtual DbSet<Post> Posts { get; set; }
		public virtual DbSet<Reation> Reations { get; set; }
		public virtual DbSet<Role> Roles { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Post>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Posts)
				.Map(m => m.ToTable("TagPost").MapLeftKey("PostId").MapRightKey("TagId"));

			modelBuilder.Entity<Role>()
				.HasMany(e => e.Users)
				.WithOptional(e => e.Role)
				.HasForeignKey(e => e.RoleId1);
		}
	}
}
