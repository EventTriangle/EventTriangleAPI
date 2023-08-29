using Microsoft.AspNetCore.SignalR;

namespace EventTriangleAPI.Consumer.BusinessLogic.Hubs;

public class NotificationHub : Hub<INotificationHub>
{
    public async Task Join()
    {
        Console.WriteLine("Hello");
    }
}