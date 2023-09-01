using EventTriangleAPI.Shared.Application.Constants;
using Microsoft.AspNetCore.SignalR;

namespace EventTriangleAPI.Consumer.BusinessLogic.Hubs.Providers;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims.First(x => x.Type == ClaimsConstants.Sub).Value;
    }
}