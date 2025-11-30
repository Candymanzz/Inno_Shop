using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService service;

        public ProductsController(IProductService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await service.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            string? userId = User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            await service.CreateAsync(product, Guid.Parse(userId));
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
        {
            string? userId = User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var updated = await service.UpdateAsync(id, product, Guid.Parse(userId));
            if (!updated)
            {
                return Forbid();
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            string? userId = User.FindFirst("userId")?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var deleted = await service.DeleteAsync(id, Guid.Parse(userId));
            if (!deleted)
            {
                return Forbid();
            }

            return Ok();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? q = null)
        {
            var result = await service.GetPagedAsync(page, pageSize, q);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? title,
            [FromQuery] Guid? userId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? minQuantity,
            [FromQuery] int? maxQuantity,
            [FromQuery] DateTime? createdAfter,
            [FromQuery] DateTime? createdBefore)
        {
            IEnumerable<Product> result = await service.SearchAsync(title, userId, minPrice, maxPrice, minQuantity, maxQuantity, createdAfter, createdBefore);
            return Ok(result);
        }

        [HttpPatch("{userId}/visibility")]
        public async Task<IActionResult> ChangeUserProductsVisibility(Guid userId, [FromQuery] bool active)
        {
            await service.SetUserProductsVisibilityAsync(userId, active);
            return Ok();
        }
    }
}
