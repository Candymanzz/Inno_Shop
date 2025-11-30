using ProductService.Domain.Common;
using ProductService.Domain.Models;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
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
            DateTime? createdBefore = null); //может можно по другому 
        Task SaveChangesAsync();
        Task ChangeUserProductsVisibilityAsync(Guid id, bool isVisible);
    }
}
