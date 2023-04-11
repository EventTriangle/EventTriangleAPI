namespace EventTriangleAPI.Shared.DTO.Responses;

public class AzureAdTokenResponse
{
    public string TokenType { get; set; }
    
    public string Scope { get; set; }
    
    public int ExpiresIn { get; set; }
    
    public int ExtExpiresIn { get; set; }
    
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}