using CSharpApp.API.Models;
using CSharpApp.Application.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDefaultConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("Error");
    app.UseHttpsRedirection();
}


app.MapGet("/todos", async (ITodoService todoService) =>
{
    var todos = await todoService.GetAllTodosAsync();
    return todos;
})
    .WithName("GetAllTodos")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService) =>
{
    var todos = await todoService.GetTodoByIdAsync(id);
    return todos;
})
    .WithName("GetTodoById")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/posts", async (IPostService postService) =>
{
    var posts = await postService.GetAllPostsAsync();
    return posts;
})
    .WithName("GetAllPosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapGet("/posts/{id}", async ([FromRoute] int id, IPostService postService) =>
{
    var post = await postService.GetPostByIdAsync(id);
    return post;
})
    .WithName("GetPostById")
    .WithOpenApi()
    .WithTags("Posts");

app.MapPost("/posts", async (PostRecordRequestModel postRequestModel, IPostService postService) =>
{
    var createPostModel = new CreatePostModel()
    {
        UserId = postRequestModel.UserId,
        Body = postRequestModel.Body,
        Title = postRequestModel.Title
    };

    var newPost = await postService.CreatePostAsync(createPostModel);
    return newPost;
})
.WithName("CreatePost")
.WithOpenApi()
.WithTags("Posts");

app.MapDelete("/posts/{id}", async ([FromRoute] int id, IPostService postService) =>
{
    var deletedPost = await postService.DeletePostAsync(id);
    return deletedPost;
})
.WithName("DeletePost")
.WithOpenApi()
.WithTags("Posts");
app.Run();
