using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Infrastructure.Date;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            string provider = configuration["DatabaseProvider"]!;

            services.AddDbContext<AppDbContext>(options =>
            {
                switch (provider)
                {
                    case "PostgreSQL": options.UseNpgsql(connectionString); break;
                    default: throw new Exception($"Unknown database provider: {provider}");
                }
            });

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
