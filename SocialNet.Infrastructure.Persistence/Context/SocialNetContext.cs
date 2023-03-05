using Microsoft.EntityFrameworkCore;
using SocialNet.Core.Domain.Core;
using SocialNet.Core.Domain.Entities;

namespace SocialNet.Infrastructure.Persistence.Context;

public class SocialNetContext : DbContext {
  public SocialNetContext(DbContextOptions<SocialNetContext> options) : base(options) { }
  public DbSet<User> Users { get; set; }
  public DbSet<Post> Posts { get; set; }
  public DbSet<Comment> Comments { get; set; }
  public DbSet<Friend> Friends { get; set; }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) {
    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
      switch (entry.State) {
        case EntityState.Added:
          entry.Entity.CreatedAt = DateTime.Now;
          entry.Entity.CreatedBy = "DefaultAppUser";
          break;
        case EntityState.Modified:
          entry.Entity.LastModifiedAt = DateTime.Now;
          entry.Entity.LastModifiedBy = "DefaultAppUser";
          break;
      }
    return base.SaveChangesAsync(cancellationToken);
  }



  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    #region  Tables
    modelBuilder.Entity<User>().ToTable("Users");
    modelBuilder.Entity<Post>().ToTable("Posts");
    modelBuilder.Entity<Comment>().ToTable("Comments");
    modelBuilder.Entity<Friend>().ToTable("Friends");
    #endregion

    #region  Primary Keys
    modelBuilder.Entity<User>().HasKey(u => u.Id);
    modelBuilder.Entity<Post>().HasKey(p => p.Id);
    modelBuilder.Entity<Comment>().HasKey(c => c.Id);
    modelBuilder.Entity<Friend>().HasKey(f => f.Id);
    #endregion

    #region  Foreign Keys
    modelBuilder.Entity<Friend>()
      .HasOne(f => f.Sender)
      .WithMany(u => u.Friends)
      .HasForeignKey(f => f.SenderId);

    modelBuilder.Entity<Post>()
      .HasOne(p => p.User)
      .WithMany(u => u.Posts)
      .HasForeignKey(p => p.UserId);

    modelBuilder.Entity<Comment>()
      .HasOne(c => c.User)
      .WithMany(u => u.Comments)
      .HasForeignKey(c => c.UserId)
      .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Comment>()
      .HasOne(c => c.Post)
      .WithMany(p => p.Comments)
      .HasForeignKey(c => c.PostId)
      .OnDelete(DeleteBehavior.Cascade);

    #endregion

    #region  "Properties configuration"

    #region  Comment
    modelBuilder.Entity<Comment>()
      .Property(c => c.Content)
      .IsRequired()
      .HasMaxLength(300);

    modelBuilder.Entity<Comment>()
      .Property(c => c.UserId)
      .IsRequired();

    modelBuilder.Entity<Comment>()
      .Property(c => c.PostId)
      .IsRequired();
    #endregion

    #region  Friend
    modelBuilder.Entity<Friend>()
      .Property(f => f.SenderId)
      .IsRequired();

    modelBuilder.Entity<Friend>()
      .Property(f => f.ReceptorId)
      .IsRequired();

    #endregion

    #region  Post

    modelBuilder.Entity<Post>()
      .Property(p => p.UserId)
      .IsRequired();

    #endregion

    #region  User
    modelBuilder.Entity<User>()
      .Property(u => u.Name)
      .IsRequired()
      .HasMaxLength(50);

    modelBuilder.Entity<User>()
      .Property(u => u.LastName)
      .IsRequired()
      .HasMaxLength(50);

    modelBuilder.Entity<User>()
      .Property(u => u.Email)
      .IsRequired()
      .HasMaxLength(50);

    modelBuilder.Entity<User>()
      .Property(u => u.Password)
      .IsRequired();

    #endregion
    #endregion
  }
}
