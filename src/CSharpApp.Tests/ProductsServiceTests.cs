using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace CSharpApp.Tests;

public class ProductsServiceTests
{
    [Fact]
    public async Task GetProducts_ReturnsProducts()
    {
        // Arrange
        var productList = new List<Product>
        {
            new Product
            {
                Id = 1,
                Title = "Test Product",
                Price = 100,
                Description = "Test Description",
                Images = new List<string> { "https://example.com/image.jpg" } // No duplicate images
            }
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(productList))
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.escuelajs.co/api/v1/")
        };

        var mockOptions = new Mock<IOptions<RestApiSettings>>();
        mockOptions.Setup(o => o.Value).Returns(new RestApiSettings
        {
            BaseUrl = "https://api.escuelajs.co/api/v1/",
            Products = "products"
        });

        var mockLogger = new Mock<ILogger<ProductsService>>();
        var service = new ProductsService(httpClient, mockOptions.Object, mockLogger.Object);

        // Act
        var products = await service.GetProducts();

        // Assert
        Assert.NotNull(products);
        Assert.Single(products);
        Assert.Equal(1, products.First().Id);
        Assert.Equal("Test Product", products.First().Title); // Use Title instead of Name
        Assert.Equal(100, products.First().Price);
        Assert.Single(products.First().Images);
        Assert.Equal("https://example.com/image.jpg", products.First().Images.First());
    }
}
