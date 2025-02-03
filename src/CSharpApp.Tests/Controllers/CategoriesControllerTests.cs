using CSharpApp.Api.Controllers;
using CSharpApp.Application.Categories.CreateCategory;
using CSharpApp.Application.Categories.GetCategories;
using CSharpApp.Application.Categories.GetCategoryById;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CSharpApp.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new CategoriesController(_mediator.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Test Category" }
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetCategoriesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCategoriesResponse(categories.AsReadOnly()));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnCategories = Assert.IsAssignableFrom<IReadOnlyCollection<Category>>(okResult.Value);
        Assert.Single(returnCategories);
    }

    [Fact]
    public async Task GetById_ReturnsOkWithCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test Category" };

        _mediator
            .Setup(m => m.Send(It.IsAny<GetCategoryByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetCategoryByIdResponse(category));

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategory = Assert.IsType<Category>(okResult.Value);
        Assert.Equal(1, returnedCategory.Id);
        Assert.Equal("Test Category", returnedCategory.Name);
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithCategory()
    {
        // Arrange
        var name = "New Category";
        var image = "test.jpg";
        var createdCategory = new Category { Id = 1, Name = name, Image = image };

        _mediator
            .Setup(m => m.Send(It.IsAny<CreateCategoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateCategoryResponse(createdCategory));

        var request = new CreateCategoryRequest(name, image);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedCategory = Assert.IsType<Category>(createdResult.Value);
        Assert.Equal(name, returnedCategory.Name);
        Assert.Equal(image, returnedCategory.Image);
        Assert.Equal(nameof(CategoriesController.GetById), createdResult.ActionName);
    }
}