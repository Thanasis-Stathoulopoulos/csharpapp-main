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
    var todos = await todoService.GetAllTodos();
    return todos;
})
    .WithName("GetAllTodos")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService) =>
{
    var todos = await todoService.GetTodoById(id);
    return todos;
})
    .WithName("GetTodoById")
    .WithOpenApi()
    .WithTags("Todos");


app.MapGet("/posts", async (IPostService postService) =>
{
    var posts = await postService.GetAllPosts();
    return posts;
})
    .WithName("GetAllPosts")
    .WithOpenApi()
    .WithTags("Posts");

app.MapGet("/posts/{id}", async ([FromRoute] int id, IPostService postService) =>
{
    var post = await postService.GetPostById(id);
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

    var newPost = await postService.CreatePost(createPostModel);
    return Results.Created($"/posts/{newPost.Id}", newPost);
})
.WithName("CreatePost")
.WithOpenApi()
.WithTags("Posts");
app.Run();
