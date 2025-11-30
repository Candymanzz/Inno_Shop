using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProductService.Application.Exceptions;
using ProductService.Domain.Common;
using ProductService.Domain.Interfaces;
using ProductService.Domain.Models;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> repoMock;
    private readonly Mock<IValidator<Product>> validatorMock;
    private readonly ProductService.Application.Services.ProductService service;

    public ProductServiceTests()
    {
        repoMock = new Mock<IProductRepository>();
        validatorMock = new Mock<IValidator<Product>>();
        service = new ProductService.Application.Services.ProductService(repoMock.Object, validatorMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct_WhenValid()
    {
        var product = new Product { Title = "Test", Price = 10, Quantity = 5 };
        validatorMock.Setup(v => v.ValidateAsync(product, default))
            .ReturnsAsync(new ValidationResult());

        await service.CreateAsync(product, Guid.NewGuid());

        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowValidationException_WhenInvalid()
    {
        var product = new Product { Title = "", Price = -1 };
        validatorMock.Setup(v => v.ValidateAsync(product, default))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Required") }));

        Func<Task> act = async () => await service.CreateAsync(product, Guid.NewGuid());

        await act.Should().ThrowAsync<ValidationException>();
        repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        Func<Task> act = async () => await service.DeleteAsync(Guid.NewGuid(), Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUserIdMismatch()
    {
        var product = new Product { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await service.DeleteAsync(product.Id, Guid.NewGuid());

        result.Should().BeFalse();
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDelete_WhenUserIdMatches()
    {
        var userId = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), UserId = userId };
        repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await service.DeleteAsync(product.Id, userId);

        result.Should().BeTrue();
        repoMock.Verify(r => r.DeleteAsync(product.Id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct()
    {
        var product = new Product { Id = Guid.NewGuid() };
        repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await service.GetByIdAsync(product.Id);

        result.Should().Be(product);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResult()
    {
        var paged = new PagedResult<Product>
        {
            Items = new List<Product> { new Product { Title = "Test" } },
            Total = 1
        };

        repoMock.Setup(r => r.GetPagedAsync(1, 10, null)).ReturnsAsync(paged);

        var result = await service.GetPagedAsync(1, 10);

        result.Should().Be(paged);
        result.Items.Should().ContainSingle(p => p.Title == "Test");
        result.Total.Should().Be(1);
    }


    [Fact]
    public async Task SearchAsync_ShouldReturnResults()
    {
        var products = new List<Product> { new Product { Title = "Test" } };
        repoMock.Setup(r => r.SearchAsync("Test", null, null, null, null, null, null, null))
            .ReturnsAsync(products);

        var result = await service.SearchAsync(title: "Test");

        result.Should().ContainSingle(p => p.Title == "Test");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        Func<Task> act = async () => await service.UpdateAsync(Guid.NewGuid(), new Product(), Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenUserIdMismatch()
    {
        var product = new Product { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await service.UpdateAsync(product.Id, new Product { Title = "New" }, Guid.NewGuid());

        result.Should().BeFalse();
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenUserIdMatches()
    {
        var userId = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), UserId = userId, Title = "Old" };
        repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await service.UpdateAsync(product.Id, new Product { Title = "New" }, userId);

        result.Should().BeTrue();
        product.Title.Should().Be("New");
        repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task SetUserProductsVisibilityAsync_ShouldCallRepository()
    {
        var userId = Guid.NewGuid();

        await service.SetUserProductsVisibilityAsync(userId, true);

        repoMock.Verify(r => r.ChangeUserProductsVisibilityAsync(userId, true), Times.Once);
    }
}
