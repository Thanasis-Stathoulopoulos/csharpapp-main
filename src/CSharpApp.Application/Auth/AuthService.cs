using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CSharpApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _settings;
    private string _cachedToken;

    public AuthService(HttpClient httpClient, IOptions<RestApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_cachedToken))
            return _cachedToken;

        var response = await _httpClient.PostAsJsonAsync(
            _settings.Auth,
            new { email = _settings.Username, password = _settings.Password }
        );

        response.EnsureSuccessStatusCode();
        var tokenResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        _cachedToken = tokenResponse.Token;
        return _cachedToken;
    }
}