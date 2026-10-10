var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Our initial inventory
var products = new List<Product>
{
    new(1, "Laptop", 899.99m, 15),
    new(2, "Keyboard", 49.99m, 30),
    new(3, "Mouse", 24.99m, 50)
};

// Application status
app.MapGet("/", () => "Inventory API is running!");

// Retrieve all products
app.MapGet("/products", () =>
{
    return Results.Ok(products);
});

// Retrieve a product by ID
app.MapGet("/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    if (product is null)
        return Results.NotFound();

    return Results.Ok(product);
});

// Add a new product
app.MapPost("/products", (Product product) =>
{
    if (products.Any(p => p.Id == product.Id))
        return Results.Conflict("Product ID already exists.");

    products.Add(product);

    return Results.Created($"/products/{product.Id}", product);
});

app.Run();

// Product data structure
record Product(int Id, string Name, decimal Price, int Stock);

public partial class Program { }
