using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace CSharpApp.Infrastructure.Services;

public class CategoriesService : ICategoriesService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;

    public CategoriesService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories()
    {
        var response = await _httpClient.GetAsync(_restApiSettings.Categories);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Category>>(content).AsReadOnly();
    }

    public async Task<Category> GetCategoryById(int id)
    {
        var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Category>(content);
    }

    public async Task<Category> CreateCategory(string name, string image)
    {
        var createCategoryRequest = new
        {
            name = name,
            image = image
        };

        var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Categories, createCategoryRequest);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Category>(content);
    }
}