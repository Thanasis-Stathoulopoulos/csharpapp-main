using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using CSharpApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CSharpApp.Tests.Services;

public class CategoriesServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IOptions<RestApiSettings>> _mockOptions;
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _settings;

    public CategoriesServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockOptions = new Mock<IOptions<RestApiSettings>>();

        _settings = new RestApiSettings
        {
            BaseUrl = "https://api.escuelajs.co/api/v1/",
            Categories = "categories"
        };

        _mockOptions.Setup(x => x.Value).Returns(_settings);
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.BaseUrl)
        };
    }

    [Fact]
    public async Task GetCategories_ReturnsCategories()
    {
        // Arrange
        var categoryList = new List<Category>
        {
            new()
            {
                Id = 1,
                Name = "Test Category",
                Image = "https://example.com/image.jpg"
            }
        };

        SetupMockHttpHandler(HttpMethod.Get, categoryList);

        var service = new CategoriesService(_httpClient, _mockOptions.Object);

        // Act
        var categories = await service.GetCategories();

        // Assert
        Assert.NotNull(categories);
        Assert.Single(categories);
        var category = categories.First();
        Assert.Equal(1, category.Id);
        Assert.Equal("Test Category", category.Name);
        Assert.Equal("https://example.com/image.jpg", category.Image);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsCategory()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Image = "https://example.com/image.jpg"
        };

        SetupMockHttpHandler(HttpMethod.Get, category);

        var service = new CategoriesService(_httpClient, _mockOptions.Object);

        // Act
        var result = await service.GetCategoryById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Category", result.Name);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "New Category",
            Image = "https://example.com/image.jpg"
        };

        SetupMockHttpHandler(HttpMethod.Post, category);

        var service = new CategoriesService(_httpClient, _mockOptions.Object);

        // Act
        var result = await service.CreateCategory("New Category", "https://example.com/image.jpg");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Category", result.Name);
        Assert.Equal("https://example.com/image.jpg", result.Image);
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