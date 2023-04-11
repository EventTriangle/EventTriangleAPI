using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Authorization.BusinessLogic.Services;
using EventTriangleAPI.Shared.Application.Services;

namespace EventTriangleAPI.Authorization.Presentation;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();

        serviceCollection.AddGrpc();

        serviceCollection.AddHttpClient();

        serviceCollection.AddSingleton<AppSettingsService>();
        serviceCollection.AddSingleton<IAzureAdService, AzureAdService>();
    }
    
    public void Configure(IApplicationBuilder applicationBuilder, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();
        }

        applicationBuilder.UseHttpsRedirection();

        applicationBuilder.UseRouting();
        
        applicationBuilder.UseAuthorization();

        applicationBuilder.UseEndpoints(option =>
        {
            option.MapControllers();
        });
    }
}