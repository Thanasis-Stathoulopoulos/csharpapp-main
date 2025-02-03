namespace CSharpApp.Core.Dtos;

public class AuthResponse
{
    [JsonPropertyName("access_token")]
    public string Token { get; set; }
}
