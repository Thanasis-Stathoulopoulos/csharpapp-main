using CSharpApp.Application.Products.CreateProduct;
using CSharpApp.Core.Dtos;
using CSharpApp.Infrastructure.Middleware;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);
builder.Services.AddDefaultConfiguration();
builder.Services.AddApiVersioning();
builder.Services.AddControllers();
builder.Services.AddHttpConfiguration();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductRequest).Assembly));
builder.Services.AddTransient<PerformanceLoggingMiddleware>();
builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<PerformanceLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();