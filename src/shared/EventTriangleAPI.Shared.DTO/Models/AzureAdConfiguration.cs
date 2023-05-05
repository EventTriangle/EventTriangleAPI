namespace EventTriangleAPI.Shared.DTO.Models;

public class AzureAdConfiguration
{
    public string Authority { get; set; }
    
    public string Instance { get; set; }
    
    public Guid TenantId { get; set; }
    
    public Guid ClientId { get; set; }
    
    public string Scopes { get; set; }
    
    public string RedirectUri { get; set; }
    
    public string CallbackPath { get; set; }
    
    public string ClientSecret { get; set; }
    
    public string AzureAdTokenUrl { get; set; }
}