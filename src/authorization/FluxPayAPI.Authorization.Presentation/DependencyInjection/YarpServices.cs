using System.Net.Http.Headers;
using FluxPayAPI.Authorization.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;
using static IdentityModel.OidcConstants;

namespace FluxPayAPI.Authorization.Presentation.DependencyInjection;

public static class YarpServices
{
    public static IServiceCollection ConfigureYarp(
        this IServiceCollection serviceCollection,
        IConfigurationSection reverseProxySection)
    {
        serviceCollection
            .AddReverseProxy()
            .LoadFromConfig(reverseProxySection)
            .AddTransforms(transformBuilderContext =>
            {
                transformBuilderContext.AddRequestTransform(async transformContext =>
                {
                    var authenticateResult = await transformContext.HttpContext.AuthenticateAsync(AuthConstants.AppOidc);
                    var accessToken = authenticateResult.Properties?.GetTokenValue(TokenTypes.AccessToken);
                    transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                });
            });

        return serviceCollection;
    }
}