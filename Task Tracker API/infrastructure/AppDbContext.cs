using Microsoft.EntityFrameworkCore;
using Task_Tracker_API.domain;

namespace TaskTracker.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.Role).HasDefaultValue("User");
        });

        b.Entity<TaskItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(160);
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.Priority).HasConversion<string>();
            e.HasIndex(x => new { x.Status, x.AssigneeId, x.CreatedAt });

            // ЯВНОЕ сопоставление навигаций:
            e.HasOne(t => t.Author)
                .WithMany()               // нет обратной коллекции у User
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.Assignee)
                .WithMany()               // нет обратной коллекции у User
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        b.Entity<Comment>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Body).IsRequired();
            e.HasOne<TaskItem>().WithMany(x => x.Comments)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>().WithMany()
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.TaskId, x.CreatedAt });
        });
    }
}