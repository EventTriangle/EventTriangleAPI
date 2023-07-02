using System.IdentityModel.Tokens.Jwt;
using EventTriangleAPI.Authorization.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
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
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromDays(2);
            })
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
                options.UsePkce = true;
                options.Scope.Clear();
                options.Scope.Add(scopes);
                options.UseTokenLifetime = true;
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.NonceCookie.SameSite = SameSiteMode.None;
                options.NonceCookie.IsEssential = true;
                
                // to map params of the token properly
                var jwtHandler = new JwtSecurityTokenHandler();
                jwtHandler.InboundClaimTypeMap.Clear();
                options.SecurityTokenValidator = jwtHandler;
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
                options.TokenValidationParameters.AuthenticationType = AuthConstants.AppOidc;
                options.DisableTelemetry = true;
                
                options.ClaimActions.Clear();
                
                // delete unused claims
                options.ClaimActions.DeleteClaim("nonce");
                options.ClaimActions.DeleteClaim("aud");
                options.ClaimActions.DeleteClaim("azp");
                options.ClaimActions.DeleteClaim("acr");
                options.ClaimActions.DeleteClaim("iss");
                options.ClaimActions.DeleteClaim("iat");
                options.ClaimActions.DeleteClaim("nbf");
                options.ClaimActions.DeleteClaim("exp");
                options.ClaimActions.DeleteClaim("at_hash");
                options.ClaimActions.DeleteClaim("c_hash");
                options.ClaimActions.DeleteClaim("ipaddr");
                options.ClaimActions.DeleteClaim("platf");
                options.ClaimActions.DeleteClaim("ver");
                options.ClaimActions.DeleteClaim("aio");
                options.ClaimActions.DeleteClaim("idp");
                options.ClaimActions.DeleteClaim("prov_data");
                options.ClaimActions.DeleteClaim("rh");
                options.ClaimActions.DeleteClaim("tid");
                options.ClaimActions.DeleteClaim("uti");
                
                // map claims
                options.ClaimActions.MapUniqueJsonKey("sub", "sub");
                options.ClaimActions.MapUniqueJsonKey("name", "name");
                options.ClaimActions.MapUniqueJsonKey("given_name", "given_name");
                options.ClaimActions.MapUniqueJsonKey("family_name", "family_name");
                options.ClaimActions.MapUniqueJsonKey("profile", "profile");
                options.ClaimActions.MapUniqueJsonKey("email", "email");
            });
        
        return serviceCollection;
    }
}