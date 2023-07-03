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
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .Build();
            });
        });

        return serviceCollection;
    }
}