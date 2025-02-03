using CSharpApp.Application.Auth;
using CSharpApp.Infrastructure.Handlers;
using CSharpApp.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        // Register AuthService
        services.AddHttpClient<IAuthService, AuthService>((provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl!);
        });

        // Register the DelegatingHandler
        services.AddTransient<AuthDelegatingHandler>();

        // Configure ProductsService with auth and retry policies
        services.AddHttpClient<IProductsService, ProductsService>((provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl!);
        })
        .AddHttpMessageHandler<AuthDelegatingHandler>()
        .AddPolicyHandler(GetRetryPolicy());

        // Configure CategoriesService similarly
        services.AddHttpClient<ICategoriesService, CategoriesService>((provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<RestApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl!);
        })
        .AddHttpMessageHandler<AuthDelegatingHandler>()
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