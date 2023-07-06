using EventTriangleAPI.Shared.Application.Enums.Events;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }
    
    public UserEventType UserEventType { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
}