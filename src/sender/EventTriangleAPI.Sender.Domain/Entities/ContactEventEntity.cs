using EventTriangleAPI.Shared.Application.Enums.Events;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class ContactEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid UserId { get; private set; }
    
    public Guid ContactId { get; private set; }
    
    public ContactEventType ContactEventType { get; private set; }
}