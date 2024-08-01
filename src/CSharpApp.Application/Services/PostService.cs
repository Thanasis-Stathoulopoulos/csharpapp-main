namespace CSharpApp.Application.Services;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly HttpClient _client;

    public PostService(ILogger<PostService> logger,
        IConfiguration configuration, HttpClient client)
    {
        _logger = logger;
        _client = client;

        var baseUrl = configuration["BaseUrl"] ?? throw new ArgumentException("BaseUrl configuration is missing or empty.");
        _client.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ReadOnlyCollection<PostRecord>?> GetAllPosts()
    {
        try
        {
            var postRecords = await _client.GetAsync("posts");

            if (postRecords.IsSuccessStatusCode)
            {
                var response = await postRecords.Content.ReadFromJsonAsync<List<PostRecord>>();

                if (response == null)
                    return default;

                return response.AsReadOnly();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {postRecords.StatusCode} ({postRecords.ReasonPhrase})");
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching posts");
            throw new Exception(ex.Message);
        }
    }
}