using CSharpApp.Core.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace CSharpApp.Infrastructure.Services;
public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(
        HttpClient httpClient,
        IOptions<RestApiSettings> restApiSettings,
        ILogger<ProductsService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        var response = await _httpClient.GetAsync(_restApiSettings.Products);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<Product>>(content);
        return products?.AsReadOnly() ?? new List<Product>().AsReadOnly();
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<Product>(content);
        return product ?? throw new InvalidOperationException($"product with id {id} not found");
    }

    public async Task<Product> CreateProduct(string title, int price, string description, List<string> images, int categoryId)
    {
        var createProductRequest = new
        {
            title = title,
            price = price,
            description = description,
            images = images,
            categoryId = categoryId
        };

        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, createProductRequest);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var prodct = JsonSerializer.Deserialize<Product>(content);
        return prodct ?? throw new InvalidOperationException("Product creation response was empty or invalid.");
    }
}