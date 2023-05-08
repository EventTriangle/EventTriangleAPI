namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class CorsServices
{
    public const string CorsPolicyName = "CorsPolicyName"; 
    
    public static IServiceCollection ConfigureCors(this IServiceCollection serviceCollection, string allowedHosts)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, corsPolicyBuilder =>
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