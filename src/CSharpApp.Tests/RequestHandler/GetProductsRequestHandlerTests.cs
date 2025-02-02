using CSharpApp.Application.Products.GetProducts;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace CSharpApp.Tests.RequestHandler;

public class GetProductsRequestHandlerTests
{
    private readonly Mock<IProductsService> _mockProductsService;
    private readonly GetProductsRequestHandler _handler;

    public GetProductsRequestHandlerTests()
    {
        _mockProductsService = new Mock<IProductsService>();
        _handler = new GetProductsRequestHandler(_mockProductsService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Title = "Test Product" }
        };

        _mockProductsService
            .Setup(x => x.GetProducts())
            .ReturnsAsync(products.AsReadOnly());

        // Act
        var result = await _handler.Handle(new GetProductsRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Products);
        Assert.Equal(1, result.Products.First().Id);
        Assert.Equal("Test Product", result.Products.First().Title);
    }
}