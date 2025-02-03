using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using CSharpApp.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CSharpApp.Tests.Services;

public class ProductsServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IOptions<RestApiSettings>> _mockOptions;
    private readonly Mock<ILogger<ProductsService>> _mockLogger;
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _settings;

    public ProductsServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockOptions = new Mock<IOptions<RestApiSettings>>();
        _mockLogger = new Mock<ILogger<ProductsService>>();

        _settings = new RestApiSettings
        {
            BaseUrl = "https://api.escuelajs.co/api/v1/",
            Products = "products"
        };

        _mockOptions.Setup(x => x.Value).Returns(_settings);
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };
    }

    [Fact]
    public async Task GetProducts_ReturnsProducts()
    {
        // Arrange
        var productList = new List<Product>
        {
            new()
            {
                Id = 1,
                Title = "Test Product",
                Price = 100,
                Description = "Test Description",
                Images = new List<string> { "https://example.com/image.jpg" }
            }
        };

        SetupMockHttpHandler(HttpMethod.Get, productList);

        var service = new ProductsService(_httpClient, _mockOptions.Object, _mockLogger.Object);

        // Act
        var products = await service.GetProducts();

        // Assert
        Assert.NotNull(products);
        Assert.Single(products);
        var product = products.First();
        Assert.Equal(1, product.Id);
        Assert.Equal("Test Product", product.Title);
        Assert.Equal(100, product.Price);
        Assert.Single(product.Images);
        Assert.Equal("https://example.com/image.jpg", product.Images.First());
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Title = "Test Product",
            Price = 100,
            Description = "Test Description",
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        SetupMockHttpHandler(HttpMethod.Get, product);

        var service = new ProductsService(_httpClient, _mockOptions.Object, _mockLogger.Object);

        // Act
        var result = await service.GetProductById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Product", result.Title);
        Assert.Equal(100, result.Price);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Title = "New Product",
            Price = 100,
            Description = "Test Description",
            Images = new List<string> { "https://example.com/image.jpg" }
        };

        SetupMockHttpHandler(HttpMethod.Post, product);

        var service = new ProductsService(_httpClient, _mockOptions.Object, _mockLogger.Object);

        // Act
        var result = await service.CreateProduct(
            "New Product",
            100,
            "Test Description",
            new List<string> { "https://example.com/image.jpg" },
            1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Product", result.Title);
        Assert.Equal(100, result.Price);
    }

    private void SetupMockHttpHandler<T>(HttpMethod method, T response)
    {
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == method),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(response))
            });
    }
}