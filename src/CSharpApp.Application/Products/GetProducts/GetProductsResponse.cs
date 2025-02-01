namespace CSharpApp.Application.Products.GetProducts;

public record GetProductsResponse(IReadOnlyCollection<Product> Products);