using MessageApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageApp.Backend.Data
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Message entity
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}