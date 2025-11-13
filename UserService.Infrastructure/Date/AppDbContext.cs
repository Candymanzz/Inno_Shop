using Microsoft.EntityFrameworkCore;
using UserService.Domain.Model;
using UserService.Infrastructure.Date.Configuration;

namespace UserService.Infrastructure.Date
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
