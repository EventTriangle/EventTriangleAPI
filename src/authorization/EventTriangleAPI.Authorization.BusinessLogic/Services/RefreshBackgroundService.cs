using EventTriangleAPI.Authorization.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace EventTriangleAPI.Authorization.BusinessLogic.Services;

public class RefreshBackgroundService : IHostedService
{
    private readonly DatabaseContext _context;
    private readonly TicketStore _ticketStore;

    public RefreshBackgroundService(DatabaseContext context, TicketStore ticketStore)
    {
        _context = context;
        _ticketStore = ticketStore;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var _ = StartRefreshingUserSessionsAsync(cancellationToken);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task StartRefreshingUserSessionsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var expiringUserSessions = await _context.UserSessions
                .AsNoTracking()
                .Where(x => (x.ExpiresAt > DateTimeOffset.UtcNow &&
                             x.ExpiresAt < DateTimeOffset.UtcNow.AddMinutes(5)) ||
                            x.ExpiresAt < DateTimeOffset.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var userSession in expiringUserSessions)
            {
                var differenceBetweenLastAccessAndUtcNow = userSession.DateOfLastAccess
                    .Subtract(DateTimeOffset.UtcNow)
                    .Duration();
                
                if (differenceBetweenLastAccessAndUtcNow > TimeSpan.FromDays(3))
                {
                    await _ticketStore.RemoveAsync(userSession.Id.ToString());
                    continue;
                }
                
                await _ticketStore.RenewAsync(userSession.Id.ToString(), null);
            }   

            await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
        }
    }
}