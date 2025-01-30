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

//app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();

var productsGroup = app.MapGroup("api/v{version:apiVersion}/products")
    .WithTags("Products")
    .WithOpenApi();

productsGroup.MapGet("/", async (IProductsService productsService) =>
    await productsService.GetProducts())
    .WithName("GetProducts")
    .Produces<IReadOnlyCollection<Product>>(StatusCodes.Status200OK);

productsGroup.MapGet("/{id}", async (int id, IProductsService productsService) =>
    await productsService.GetProductById(id))
    .WithName("GetProductById")
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

productsGroup.MapPost("/", async (Product product, IProductsService productsService) =>
    Results.CreatedAtRoute("GetProductById", new { id = (await productsService.CreateProduct(product)).Id }, product))
    .WithName("CreateProduct")
    .Produces<Product>(StatusCodes.Status201Created)
    .ProducesValidationProblem();

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