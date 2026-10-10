using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InventoryApi.Tests;

public class InventoryApiTests
{
    [Fact]
    public async Task GetRoot_ReturnsSuccess()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_ReturnsThreeProducts()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var products = await client.GetFromJsonAsync<List<ProductResponse>>("/products");

        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
    }

    [Fact]
    public async Task GetProductById_ReturnsCorrectProduct()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var product = await client.GetFromJsonAsync<ProductResponse>("/products/2");

        Assert.NotNull(product);
        Assert.Equal("Keyboard", product.Name);
    }

    [Fact]
    public async Task GetMissingProduct_ReturnsNotFound()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/products/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
public async Task CreateProduct_ReturnsCreated()
{
    await using var factory = new WebApplicationFactory<Program>();
    using var client = factory.CreateClient();

    var newProduct = new ProductResponse(4, "Monitor", 199.99m, 12);

    var response = await client.PostAsJsonAsync("/products", newProduct);

    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
}

[Fact]
public async Task CreateDuplicateProduct_ReturnsConflict()
{
    await using var factory = new WebApplicationFactory<Program>();
    using var client = factory.CreateClient();

    var duplicateProduct = new ProductResponse(1, "Laptop", 899.99m, 15);

    var response = await client.PostAsJsonAsync("/products", duplicateProduct);

    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
}
}

public record ProductResponse(int Id, string Name, decimal Price, int Stock);
