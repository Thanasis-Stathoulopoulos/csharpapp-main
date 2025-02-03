using CSharpApp.Application.Categories.GetCategoryById;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Moq;

namespace CSharpApp.Tests.RequestHandler;

public class GetCategoryByIdRequestHandlerTests
{
    private readonly Mock<ICategoriesService> _mockCategoriesService;
    private readonly GetCategoryByIdRequestHandler _handler;

    public GetCategoryByIdRequestHandlerTests()
    {
        _mockCategoriesService = new Mock<ICategoriesService>();
        _handler = new GetCategoryByIdRequestHandler(_mockCategoriesService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };

        _mockCategoriesService
            .Setup(x => x.GetCategoryById(1))
            .ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(new GetCategoryByIdRequest(1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Category.Id);
        Assert.Equal("Test Category", result.Category.Name);
    }
}