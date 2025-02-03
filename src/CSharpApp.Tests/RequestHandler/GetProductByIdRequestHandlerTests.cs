using CSharpApp.Application.Products.GetProductById;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Moq;

namespace CSharpApp.Tests.RequestHandler;

public class GetProductByIdRequestHandlerTests
{
    private readonly Mock<IProductsService> _mockProductsService;
    private readonly GetProductByIdRequestHandler _handler;

    public GetProductByIdRequestHandlerTests()
    {
        _mockProductsService = new Mock<IProductsService>();
        _handler = new GetProductByIdRequestHandler(_mockProductsService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Title = "Test Product" };

        _mockProductsService
            .Setup(x => x.GetProductById(1))
            .ReturnsAsync(product);

        // Act
        var result = await _handler.Handle(new GetProductByIdRequest(1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Product.Id);
        Assert.Equal("Test Product", result.Product.Title);
    }
}