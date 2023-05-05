namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class CorsServices
{
    public static IServiceCollection ConfigureCors(
        this IServiceCollection serviceCollection,
        string corsPolicyName,
        string allowedHosts)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .WithOrigins(allowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return serviceCollection;
    }
}