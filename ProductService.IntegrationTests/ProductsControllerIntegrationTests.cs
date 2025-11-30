using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductService.Domain.Models;

public class ProductsControllerIntegrationTests
    : IClassFixture<WebApplicationFactory<ProductService.API.Program>>
{
    private readonly HttpClient client;

    public ProductsControllerIntegrationTests(WebApplicationFactory<ProductService.API.Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var response = await client.GetAsync($"/api/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnUnauthorized_WhenNoToken()
    {
        var product = new Product { Title = "Test", Price = 10, Quantity = 5 };

        var response = await client.PostAsJsonAsync("/api/products", product);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenAuthorized()
    {
        var product = new Product { Title = "Test", Price = 10, Quantity = 5 };

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TestJwtHelper.GenerateToken());

        var response = await client.PostAsJsonAsync("/api/products", product);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
