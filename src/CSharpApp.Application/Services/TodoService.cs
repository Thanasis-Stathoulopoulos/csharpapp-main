namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClient _client;

    public TodoService(ILogger<TodoService> logger,
        IConfiguration configuration, HttpClient client)
    {
        _logger = logger;
        _client = client;

        var baseUrl = configuration["BaseUrl"] ?? throw new ArgumentException("BaseUrl configuration is missing or empty.");
        _client.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ReadOnlyCollection<TodoRecord>?> GetAllTodos()
    {
        try
        {
            var todoRecords = await _client.GetAsync("todos");

            if (todoRecords.IsSuccessStatusCode)
            {
                var response = await todoRecords.Content.ReadFromJsonAsync<List<TodoRecord>>();

                if (response == null)
                    return default;

                return response.AsReadOnly();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {todoRecords.StatusCode} ({todoRecords.ReasonPhrase})");
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo items");
            throw new Exception(ex.Message);
        }
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        try
        {
            var todoRecord = await _client.GetAsync($"todos/{id}");

            if (todoRecord.IsSuccessStatusCode)
            {
                var response = await todoRecord.Content.ReadFromJsonAsync<TodoRecord>();

                return response;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {todoRecord.StatusCode} ({todoRecord.ReasonPhrase})");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo with item ID: {ItemId}", id);
            throw new Exception(ex.Message);
        }
    }
}