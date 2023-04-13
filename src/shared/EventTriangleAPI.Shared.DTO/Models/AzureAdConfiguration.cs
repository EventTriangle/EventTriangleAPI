namespace EventTriangleAPI.Shared.DTO.Models;

public class AzureAdConfiguration
{
    public string Instance { get; set; }
    public Guid TenantId { get; set; }
    public Guid ClientId { get; set; }
    public string Scopes { get; set; }
}