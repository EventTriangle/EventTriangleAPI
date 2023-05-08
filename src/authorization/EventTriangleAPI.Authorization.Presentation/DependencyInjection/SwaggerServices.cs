using Microsoft.OpenApi.Models;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class SwaggerServices
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "EventTriangle Authorization API", Version = "v1" });
        });

        return serviceCollection;
    }
}