using CSharpApp.Api.Controllers;
using CSharpApp.Application.Products.GetProducts;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CSharpApp.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOkWithProducts()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var products = new List<Product>
        {
            new Product { Id = 1, Title = "Test" }
        };

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetProductsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetProductsResponse(products.AsReadOnly()));

            var controller = new ProductsController(mockMediator.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IReadOnlyCollection<Product>>(okResult.Value);
            Assert.Single(returnProducts);
        }
    }
}
