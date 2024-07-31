using CSharpApp.Application.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDefaultConfiguration();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure TodoService with HttpClient
builder.Services.AddHttpClient<ITodoService, TodoService>()
    .ConfigureHttpClient((serviceProvider, client) =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var baseUrl = configuration["BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("Base URL must be configured.");
        }
        client.BaseAddress = new Uri(baseUrl);
    });

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error"); // Customize your error handling
    app.UseHttpsRedirection();
}

app.MapGet("/todos", async (ITodoService todoService) =>
{
    var todos = await todoService.GetAllTodos();
    return todos;
})
.WithName("GetTodos")
.WithOpenApi();

app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService) =>
{
    var todos = await todoService.GetTodoById(id);
    return todos;
})
.WithName("GetTodosById")
.WithOpenApi();

app.Run();