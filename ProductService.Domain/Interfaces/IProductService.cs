using ProductService.Domain.Common;
using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(Product product, Guid userId);
        Task<bool> UpdateAsync(Guid id, Product product, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
        Task<Product?> GetByIdAsync(Guid id);
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, string? q = null);
        Task<IEnumerable<Product>> SearchAsync(
            string? title = null,
            Guid? userId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? minQuantity = null,
            int? maxQuantity = null,
            DateTime? createdAfter = null,
            DateTime? createdBefore = null);
        Task SetUserProductsVisibilityAsync(Guid userId, bool isVisible);
    }
}
