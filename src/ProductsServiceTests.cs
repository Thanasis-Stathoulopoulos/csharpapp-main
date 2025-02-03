using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CSharpApp.Tests;

public class ProductsServiceTests
{
    [Fact]
    public async Task GetProducts_ReturnsProducts()
    {
        // Arrange
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
                Content = new StringContent(JsonSerializer.Serialize(new List<Product>
                {
                    new Product { Id = 1, Name = "Test Product" }
                }))
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

        var service = new ProductsService(httpClient, mockOptions.Object, Mock.Of<ILogger<ProductsService>>());

        // Act
        var products = await service.GetProducts();

        // Assert
        Assert.NotNull(products);
        Assert.Single(products); // Verify one product was returned
        Assert.Equal(1, products.First().Id);
    }
}