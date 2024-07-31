namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClient _client;

    public TodoService(ILogger<TodoService> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        try
        {
            var response = await _client.GetFromJsonAsync<TodoRecord>($"todos/{id}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo with item ID: {Id}", id);
            return null;
        }
    }
   
    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        try
        {
            var response = await _client.GetFromJsonAsync<List<TodoRecord>>("todos");
            return response?.AsReadOnly() ?? new List<TodoRecord>().AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all todo items");
            return new List<TodoRecord>().AsReadOnly();
        }
    }
}
