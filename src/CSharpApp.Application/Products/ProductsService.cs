using System.Net.Http.Json;
namespace CSharpApp.Application.Products;
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
        return JsonSerializer.Deserialize<List<Product>>(content).AsReadOnly();
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Product>(content);
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, product);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Product>(content);
    }
}