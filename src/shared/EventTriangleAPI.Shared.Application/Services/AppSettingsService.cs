namespace EventTriangleAPI.Shared.Application.Services;

public class AppSettingsService
{
    private const string AppSettingsPathSender = "../../sender/EventTriangleAPI.Sender.Presentation/appsettings.json";

    private const string AppSettingsPathConsumer = "../../consumer/EventTriangleAPI.Consumer.Presentation/appsettings.json";
    
    public string GetAppSettingsPathSender()
    {
        var workingDirectory = Environment.CurrentDirectory;
        
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        
        var path = Path.Combine(projectDirectory, AppSettingsPathSender);
        
        return path;
    }
    
    public string GetAppSettingsPathConsumer()
    {
        var workingDirectory = Environment.CurrentDirectory;
        
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        
        var path = Path.Combine(projectDirectory, AppSettingsPathConsumer);
        
        return path;
    }
}