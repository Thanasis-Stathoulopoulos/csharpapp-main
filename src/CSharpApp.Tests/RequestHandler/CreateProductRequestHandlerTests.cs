using CSharpApp.Application.Products.CreateProduct;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Moq;

namespace CSharpApp.Tests.RequestHandler;

public class CreateProductRequestHandlerTests
{
    private readonly Mock<IProductsService> _mockProductsService;
    private readonly CreateProductRequestHandler _handler;

    public CreateProductRequestHandlerTests()
    {
        _mockProductsService = new Mock<IProductsService>();
        _handler = new CreateProductRequestHandler(_mockProductsService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCreatedProduct()
    {
        // Arrange
        var title = "New Product";
        var price = 100;
        var description = "Test Description";
        var images = new List<string> { "test.jpg" };
        var categoryId = 1;

        var createdProduct = new Product
        {
            Id = 1,
            Title = title,
            Price = price,
            Description = description,
            Images = images
        };

        _mockProductsService
            .Setup(x => x.CreateProduct(title, price, description, images, categoryId))
            .ReturnsAsync(createdProduct);

        var request = new CreateProductRequest(title, price, description, images, categoryId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(title, result.Product.Title);
        Assert.Equal(price, result.Product.Price);
    }
}