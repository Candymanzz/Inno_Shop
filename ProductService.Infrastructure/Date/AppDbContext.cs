using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Date.Configuration;

namespace ProductService.Infrastructure.Date
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
