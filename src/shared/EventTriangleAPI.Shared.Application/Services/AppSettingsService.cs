namespace EventTriangleAPI.Shared.Application.Services;

public class AppSettingsService
{
    private const string AppSettingsPathSender = "../../../../../sender/EventTriangleAPI.Sender.Presentation/appsettings.json";

    private const string AppSettingsPathConsumer = "../../../../../consumer/EventTriangleAPI.Consumer.Presentation/appsettings.json";

    private const string AppSettingsPathAuthorization = "../../../../../authorization/EventTriangleAPI.Authorization.Presentation/appsettings.json";
    
    public string GetAppSettingsPathSender()
    {
        var path = Path.Combine(AppContext.BaseDirectory, AppSettingsPathSender);
        
        return path;
    }
    
    public string GetAppSettingsPathConsumer()
    {
        var path = Path.Combine(AppContext.BaseDirectory, AppSettingsPathConsumer);
        
        return path;
    }
    
    public string GetAppSettingsPathAuthorization()
    {
        var path = Path.Combine(AppContext.BaseDirectory, AppSettingsPathAuthorization);
        
        return path;
    }
}