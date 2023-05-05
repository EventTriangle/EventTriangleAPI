using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Yarp.ReverseProxy.Transforms;

namespace EventTriangleAPI.Authorization.Presentation.DependencyInjection;

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
                    var authenticateResult = await transformContext.HttpContext.AuthenticateAsync("appOidc");
                    var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");
                    transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                });
            });

        return serviceCollection;
    }
}