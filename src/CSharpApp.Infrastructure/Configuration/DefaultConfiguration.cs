namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ITodoService, TodoService>();
        services.AddTransient<IPostService, PostService>();

        return services;
    }
}