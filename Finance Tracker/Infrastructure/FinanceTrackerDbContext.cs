using Finance_Tracker.Models;
using Finance_Tracker.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finance_Tracker.Infrastructure
{
    public class FinanceTrackerDbContext : DbContext
    {
        public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.HasIndex(x => x.Email)
                      .IsUnique();

                entity.Property(x => x.PasswordHash)
                      .IsRequired();

                entity.Property(x => x.Role)
                      .HasDefaultValue("User");
            });

            modelBuilder.Entity<RefreshToken>().HasIndex(x => x.Token);
        }

    }
}
