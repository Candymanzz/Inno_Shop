using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateAsync(Product product) //dto
        {
            product.CreateAt = DateTime.UtcNow;
            await repository.AddAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            Product? existing = await repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception("Prduct not found.");
            }

            await repository.DeleteAsync(id);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<Product> Items, int Total)> GetPagedAsync(int page, int pageSize, string? q = null)
        {
            return await repository.GetPagedAsync(page, pageSize, q);
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
            if (repository is IProductRepository repoWithSearch)
            {
                return await repoWithSearch.SearchAsync(
                    title, userId, minPrice, maxPrice,
                    minQuantity, maxQuantity, createdAfter, createdBefore);
            }

            return Enumerable.Empty<Product>();
        }

        public async Task UpdateAsync(Product product) //dto
        {
            var existing = await repository.GetByIdAsync(product.Id);
            if (existing == null)
            {
                throw new Exception("Prduct not found.");
            }

            existing.Title = product.Title;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Quantity = product.Quantity;
            existing.UserId = product.UserId;

            await repository.UpdateAsync(existing);
        }
    }
}
