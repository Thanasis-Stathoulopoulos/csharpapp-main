namespace CSharpApp.Core.Settings;

public sealed class RestApiSettings
{
    private string? _baseUrl;
    private string? _products;
    private string? _categories;
    private string? _auth;

    public string? BaseUrl
    {
        get => _baseUrl;
        set => _baseUrl = string.IsNullOrEmpty(value)
            ? value
            : value.TrimEnd('/') + "/"; // Ensure trailing slash
    }

    public string? Products
    {
        get => _products;
        set => _products = value?.TrimStart('/'); // Trim leading slash
    }

    public string? Categories
    {
        get => _categories;
        set => _categories = value?.TrimStart('/'); // Trim leading slash
    }

    public string? Auth
    {
        get => _auth;
        set => _auth = value?.TrimStart('/'); // Trim leading slash
    }

    public string? Username { get; set; }
    public string? Password { get; set; }
}