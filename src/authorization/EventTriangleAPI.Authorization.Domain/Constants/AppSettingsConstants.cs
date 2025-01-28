namespace EventTriangleAPI.Authorization.Domain.Constants;

public static class AppSettingsConstants
{
    public const string DatabaseConnectionString = "DatabaseConnectionString";

    public const string AzureAdSelection = "AzureAd";

    public const string ReverseProxySelection = "ReverseProxy";
    
    public const string AdSecretKey = "EVENT_TRIANGLE_AD_CLIENT_SECRET";

    public const string DevFrontendUrl = "DevFrontendUrl";

    public const string AllowedOrigins = "AllowedOrigins";

    public const string GrpcChannelAddresses = "GrpcChannelAddresses";
}