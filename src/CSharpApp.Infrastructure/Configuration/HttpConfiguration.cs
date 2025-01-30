using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        // Configure ProductsService HTTP client
        services.AddHttpClient<IProductsService, ProductsService>((provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl!);
        })
        .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}