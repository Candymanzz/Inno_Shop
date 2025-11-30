using FluentValidation;
using ProductService.Application.Exceptions;
using ProductService.Domain.Common;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IValidator<Product> validator;

        public ProductService(IProductRepository repository, IValidator<Product> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public async Task CreateAsync(Product product, Guid userId)
        {
            var validationResult = await validator.ValidateAsync(product);
            if (!validationResult.IsValid) 
            {
                throw new ValidationException(validationResult.Errors);
            }

            product.UserId = userId;
            product.CreateAt = DateTime.UtcNow;

            await repository.AddAsync(product);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }  

            if (existing.UserId != userId)
            {
                return false;
            }

            await repository.DeleteAsync(id);
            return true;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, string? q = null)
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
            return await repository.SearchAsync(
                title, userId, minPrice, maxPrice,
                minQuantity, maxQuantity, createdAfter, createdBefore);
        }

        public async Task<bool> UpdateAsync(Guid id, Product product, Guid userId)
        {
            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            if (existing.UserId != userId)
            {
                return false;
            }

            existing.Title = product.Title;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Quantity = product.Quantity;

            await repository.UpdateAsync(existing);
            return true;
        }

        public async Task SetUserProductsVisibilityAsync(Guid userId, bool isVisible)
        {
            await repository.ChangeUserProductsVisibilityAsync(userId, isVisible);
        }
    }
}
