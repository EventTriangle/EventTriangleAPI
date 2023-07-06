using EventTriangleAPI.Shared.Application.Enums;
using EventTriangleAPI.Shared.Application.Enums.Events;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid CardId { get; private set; }
    
    public Guid UserId { get; private set; }

    public string HolderName { get; private set; }
    
    public string CardNumber { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; private set; }
    
    public CardEventType CardEventType { get; private set; }
}