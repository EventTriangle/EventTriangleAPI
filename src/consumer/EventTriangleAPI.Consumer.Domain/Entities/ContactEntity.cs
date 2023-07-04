using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class ContactEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; set; }
    
    public Guid ContactId { get; set; }
    
    public string ContactUsername { get; set; }
}