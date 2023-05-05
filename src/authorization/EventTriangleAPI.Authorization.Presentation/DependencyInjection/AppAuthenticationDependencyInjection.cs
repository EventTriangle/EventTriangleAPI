using EventTriangleAPI.Authorization.Presentation.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

public static class AppAuthenticationDependencyInjection
{
    public static IServiceCollection AddAppAuthentication(
        this IServiceCollection serviceCollection,
        string authority,
        string clientId,
        string clientSecret,
        string callbackPath,
        string scopes)
    {
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = AuthConstants.AppOidc;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(AuthConstants.AppOidc, options =>
            {
                options.Authority = authority;
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.CallbackPath = new PathString(callbackPath);
                options.ResponseType = "code";
                options.ResponseMode = "query";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = false;
                options.RequireHttpsMetadata = false;
                options.Scope.Clear();
                options.Scope.Add(scopes);
                options.UseTokenLifetime = true;
            });
        
        return serviceCollection;
    }
}