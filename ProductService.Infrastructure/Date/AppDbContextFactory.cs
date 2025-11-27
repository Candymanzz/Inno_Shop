using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductService.Infrastructure.Date
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            string provider = configuration["DatabaseProvider"]!;

            DbContextOptionsBuilder<AppDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            switch (provider)
            {
                case "PostgreSQL": dbContextOptionsBuilder.UseNpgsql(connectionString); break; // и тд
                default: throw new Exception($"Unknown database provider: {provider}");
            }

            return new AppDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
