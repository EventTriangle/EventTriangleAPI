using System.Reflection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace EventTriangleAPI.Consumer.Presentation.DependencyInjection;

public static class SwaggerServices
{
    private const string DefaultVersion = "1.0.0.0";
    
    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? DefaultVersion;
            
            c.EnableAnnotations();

            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = $"EventTriangle Consumer API {version}", Version = $"v{version}" });
            
            c.AddSecurityDefinition(
                "token",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "token"
                            },
                        },
                        Array.Empty<string>()
                    }
                }
            );
        });

        return serviceCollection;
    }
}