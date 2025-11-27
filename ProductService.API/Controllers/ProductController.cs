using Microsoft.AspNetCore.Mvc;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

namespace ProductService.API.Controllers
{
    public class ProductController : ControllerBase
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
                if (product == null) return NotFound();
                return Ok(product);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] Product product)
            {
                await service.CreateAsync(product);
                return Ok();
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
            {
                if (id != product.Id) return BadRequest("Id mismatch");

                await service.UpdateAsync(product);
                return Ok();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                await service.DeleteAsync(id);
                return Ok();
            }

            [HttpGet("paged")]
            public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? q = null)
            {
                (IEnumerable<Product> Items, int Total) result = await service.GetPagedAsync(page, pageSize, q);
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
        }
    }
}
