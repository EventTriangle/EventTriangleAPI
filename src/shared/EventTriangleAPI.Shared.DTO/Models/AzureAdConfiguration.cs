namespace EventTriangleAPI.Shared.DTO.Models;

public record AzureAdConfiguration(
    string Instance,
    Guid TenantId,
    Guid ClientId,
    string Scopes);