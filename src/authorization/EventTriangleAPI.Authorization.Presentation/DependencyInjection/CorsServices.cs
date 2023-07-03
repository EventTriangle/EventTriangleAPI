namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class CorsServices
{
    public const string CorsPolicyName = "CorsPolicyName"; 
    
    public static IServiceCollection ConfigureCors(this IServiceCollection serviceCollection, string[] allowedOrigins)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .AllowAnyHeader();
            });
        });

        return serviceCollection;
    }
}