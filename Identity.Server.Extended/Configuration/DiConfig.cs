using Identity.Server.Extended.Services;
using Identity.Server.Extended.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Configuration;

public static class DiConfig
{
    /// <summary>
    /// Adds the extended Identity Server DI configuration.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddExtendedIdentityServerDiConfig(this WebApplicationBuilder builder)
    {
        // Configure options.
        builder.Services.AddTransient<IClientManager, ClientManager>();
        return builder;
    }
}