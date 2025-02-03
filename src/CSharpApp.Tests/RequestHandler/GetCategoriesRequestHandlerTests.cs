using CSharpApp.Application.Categories.GetCategories;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Moq;
using Xunit;

namespace CSharpApp.Tests.RequestHandler;

public class GetCategoriesRequestHandlerTests
{
    private readonly Mock<ICategoriesService> _mockCategoriesService;
    private readonly GetCategoriesRequestHandler _handler;

    public GetCategoriesRequestHandlerTests()
    {
        _mockCategoriesService = new Mock<ICategoriesService>();
        _handler = new GetCategoriesRequestHandler(_mockCategoriesService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Test Category" }
        };

        _mockCategoriesService
            .Setup(x => x.GetCategories())
            .ReturnsAsync(categories.AsReadOnly());

        // Act
        var result = await _handler.Handle(new GetCategoriesRequest(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Categories);
        Assert.Equal(1, result.Categories.First().Id);
        Assert.Equal("Test Category", result.Categories.First().Name);
    }
}