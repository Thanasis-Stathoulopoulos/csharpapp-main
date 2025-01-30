using CSharpApp.Core.Dtos;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();   
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
{
    var products = await productsService.GetProducts();
    return products;
})
    .WithName("GetProducts")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/categories", async (ICategoriesService categoriesService) =>
{
    var categories = await categoriesService.GetCategories();
    return categories;
})
    .WithName("GetCategories")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/categories/{id}", async (int id, ICategoriesService categoriesService) =>
{
    var category = await categoriesService.GetCategoryById(id);
    return category;
})
    .WithName("GetCategoryById")
    .HasApiVersion(1.0);

versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/categories", async (Category category, ICategoriesService categoriesService) =>
{
    var createdCategory = await categoriesService.CreateCategory(category);
    return Results.Created($"categories/{createdCategory.Id}", createdCategory);
})
    .WithName("CreateCategory")
    .HasApiVersion(1.0);

app.Run();