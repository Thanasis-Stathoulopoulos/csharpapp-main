using CSharpApp.Application.Products.CreateProduct;
using CSharpApp.Core.Dtos;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();
builder.Services.AddControllers();  
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductRequest).Assembly));  

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();  // Add this
app.UseEndpoints(endpoints =>  // Add this
{
    endpoints.MapControllers();
});

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

var categoriesGroup = versionedEndpointRouteBuilder.MapGroup("api/v{version:apiVersion}/categories")
    .WithTags("Categories")
    .WithOpenApi()
    .HasApiVersion(1.0);

categoriesGroup.MapGet("/", async (ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetCategories();
    return categories;
})
.WithName("GetCategories");

categoriesGroup.MapGet("/{id}", async (int id, ICategoriesService categoriesService) =>
{
    var category = await categoriesService.GetCategoryById(id);
    return category;
})
.WithName("GetCategoryById");

categoriesGroup.MapPost("/", async (Category category, ICategoriesService categoriesService) =>
{
    var createdCategory = await categoriesService.CreateCategory(category);
    return Results.CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id, version = "1.0" }, createdCategory);
})
.WithName("CreateCategory");

app.Run();