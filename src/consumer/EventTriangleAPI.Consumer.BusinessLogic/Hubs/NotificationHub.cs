using Microsoft.AspNetCore.SignalR;

namespace EventTriangleAPI.Consumer.BusinessLogic.Hubs;

public class NotificationHub : Hub<INotificationHub>
{
    public Task Join()
    {
        Console.WriteLine("Hello");

        return Task.CompletedTask;
    }
}