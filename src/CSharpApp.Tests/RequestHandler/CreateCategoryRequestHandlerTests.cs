using CSharpApp.Application.Categories.CreateCategory;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Moq;
using Xunit;

namespace CSharpApp.Tests.RequestHandler;

public class CreateCategoryRequestHandlerTests
{
    private readonly Mock<ICategoriesService> _mockCategoriesService;
    private readonly CreateCategoryRequestHandler _handler;

    public CreateCategoryRequestHandlerTests()
    {
        _mockCategoriesService = new Mock<ICategoriesService>();
        _handler = new CreateCategoryRequestHandler(_mockCategoriesService.Object);
    }

    [Fact]
    public async Task Handle_ReturnsCreatedCategory()
    {
        // Arrange
        var name = "New Category";
        var image = "test.jpg";

        var createdCategory = new Category
        {
            Id = 1,
            Name = name,
            Image = image
        };

        _mockCategoriesService
            .Setup(x => x.CreateCategory(name, image))
            .ReturnsAsync(createdCategory);

        var request = new CreateCategoryRequest(name, image);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Category.Name);
        Assert.Equal(image, result.Category.Image);
    }
}