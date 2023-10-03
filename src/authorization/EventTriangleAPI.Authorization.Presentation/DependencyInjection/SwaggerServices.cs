using System.Reflection;
using Microsoft.OpenApi.Models;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class SwaggerServices
{
    private const string DefaultVersion = "1.0.0.0";

    public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? DefaultVersion;

            c.EnableAnnotations();

            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "EventTriangle Authorization API", Version = $"v{version}" });
        });

        return serviceCollection;
    }
}