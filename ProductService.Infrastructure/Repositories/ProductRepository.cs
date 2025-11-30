using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;
using ProductService.Infrastructure.Date;

using ProductService.Domain.Common;

namespace ProductService.Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Product? product = await GetByIdAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await SaveChangesAsync();
            }
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, string? q = null)
        {
            IQueryable<Product> query = context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Title.Contains(q) || p.Description.Contains(q));

            int total = await query.CountAsync();
            List<Product> items = await query
                .Where(p => p.IsVisible == true)
                .OrderByDescending(p => p.CreateAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                Total = total
            };
        }


        public async Task<IEnumerable<Product>> SearchAsync(
            string? title = null, 
            Guid? userId = null, 
            decimal? minPrice = null, 
            decimal? maxPrice = null, 
            int? minQuantity = null, 
            int? maxQuantity = null, 
            DateTime? createdAfter = null, 
            DateTime? createdBefore = null)
        {
            IQueryable<Product> query = context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(p => p.Title.Contains(title));

            if (userId.HasValue)
                query = query.Where(p => p.UserId == userId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => (decimal)p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => (decimal)p.Price <= maxPrice.Value);

            if (minQuantity.HasValue)
                query = query.Where(p => p.Quantity >= minQuantity.Value);

            if (maxQuantity.HasValue)
                query = query.Where(p => p.Quantity <= maxQuantity.Value);

            if (createdAfter.HasValue)
                query = query.Where(p => p.CreateAt >= createdAfter.Value);

            if (createdBefore.HasValue)
                query = query.Where(p => p.CreateAt <= createdBefore.Value);

            return await query.Where(p => p.IsVisible == true).OrderByDescending(p => p.CreateAt).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            context.Products.Update(product);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task ChangeUserProductsVisibilityAsync(Guid id, bool isVisible)
        {
            await context.Products
                .Where(p => p.UserId == id)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(x => x.IsVisible, isVisible)
                );
            await SaveChangesAsync();
        }
    }
}
