using CSharpApp.Application.Products.CreateProduct;
using CSharpApp.Core.Dtos;
using CSharpApp.Infrastructure.Middleware;
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
builder.Services.AddTransient<PerformanceLoggingMiddleware>();
builder.Services.AddTransient<ErrorHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<PerformanceLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();