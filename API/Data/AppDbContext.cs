using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        internal DbSet<Course> Courses { get; set; }
        internal DbSet<Platform> Platforms { get; set; }
        internal DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships between Course and AppUser
            modelBuilder.Entity<Course>()
                .HasOne(c => c.User)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.username)
                .HasPrincipalKey(u => u.UserName)
                .IsRequired();
        }
    }
}