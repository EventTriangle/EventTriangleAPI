using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Web;

namespace EventTriangleAPI.Consumer.BusinessLogic.Hubs.Providers;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims.First(x => x.Type == ClaimConstants.NameIdentifierId).Value;
    }
}