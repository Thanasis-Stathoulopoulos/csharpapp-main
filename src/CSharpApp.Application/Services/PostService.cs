using CSharpApp.Application.Dtos;
using CSharpApp.Application.Interfaces;
using CSharpApp.Application.Models;

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

    public async Task<PostRecordResponseModel> GetAllPostsAsync()
    {
        var result = new PostRecordResponseModel();

        try
        {
            var response = await _client.GetAsync("posts");

            if (response.IsSuccessStatusCode)
            {
                var postRecords = await response.Content.ReadFromJsonAsync<List<PostRecord>>();

                result.OperationSucceeded = true;
                result.PostRecords = postRecords;
            }
            else
            {
                string message = $"Request failed with status code {response.StatusCode} ({response.ReasonPhrase})";
                _logger.LogError(message);
                result.OperationSucceeded = false;
                result.ErrorResults =
                [
                    new($"{response.StatusCode}", $"{response.ReasonPhrase}")
                ];
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching posts");
            result.ErrorResults =
            [
                new($"{ex.StatusCode}", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching posts");
            result.ErrorResults =
            [
              new("500", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        return result;
    }

    public async Task<PostRecordResponseModel> GetPostByIdAsync(int id)
    {
        var result = new PostRecordResponseModel();

        try
        {
            var response = await _client.GetAsync($"posts/{id}");

            if (response.IsSuccessStatusCode)
            {
                var postRecord = await response.Content.ReadFromJsonAsync<PostRecord>();

                var record = new List<PostRecord>
                {
                    postRecord
                };

                result.OperationSucceeded = true;
                result.PostRecords = record;

            }
            else
            {
                string message = $"Request failed with status code {response.StatusCode} ({response.ReasonPhrase})";
                _logger.LogError(message);
                result.OperationSucceeded = false;
                result.ErrorResults =
                [
                    new($"{response.StatusCode}", $"{response.ReasonPhrase}")
                ];
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching posts");
            result.ErrorResults =
            [
                new($"{ex.StatusCode}", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching posts");
            result.ErrorResults =
            [
              new("500", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        return result;
    }

    public async Task<PostRecordResponseModel> CreatePostAsync(CreatePostModel createPostModel)
    {
        var result = new PostRecordResponseModel();

        try
        {
            var createdPost = await _client.PostAsJsonAsync("posts", createPostModel);

            if (createdPost.IsSuccessStatusCode)
            {
                result.OperationSucceeded = true;
            }
            else
            {
                string message = $"Request failed with status code {createdPost.StatusCode} ({createdPost.ReasonPhrase})";
                _logger.LogError(message);
                result.OperationSucceeded = false;
                result.ErrorResults =
                [
                    new($"{createdPost.StatusCode}", $"{createdPost.ReasonPhrase}")
                ];
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching posts");
            result.ErrorResults =
            [
                new($"{ex.StatusCode}", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching posts");
            result.ErrorResults =
            [
              new("500", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        return result;
    }

    public async Task<PostRecordResponseModel> DeletePostAsync(int id)
    {
        var result = new PostRecordResponseModel();

        try
        {
            var postRecord = await _client.DeleteAsync($"posts/{id}");

            if (postRecord.IsSuccessStatusCode)
            {
                result.OperationSucceeded = true;
            }
            else
            {
                string message = $"Request failed with status code {postRecord.StatusCode} ({postRecord.ReasonPhrase})";
                _logger.LogError(message);
                result.OperationSucceeded = false;
                result.ErrorResults =
                [
                    new($"{postRecord.StatusCode}", $"{postRecord.ReasonPhrase}")
                ];
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching posts");
            result.ErrorResults =
            [
                new($"{ex.StatusCode}", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching posts");
            result.ErrorResults =
            [
              new("500", ex.Message)
            ];
            result.OperationSucceeded = false;
        }
        return result;
    }
}
