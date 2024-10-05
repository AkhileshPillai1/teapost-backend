using Microsoft.EntityFrameworkCore;
using TeaPost.Models;

namespace TeaPost.DatabaseConnection
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the composite primary key
            modelBuilder.Entity<Followers>()
                .HasKey(f => new { f.Follower, f.Followed });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Followers> Followers { get; set; }
    }
}
