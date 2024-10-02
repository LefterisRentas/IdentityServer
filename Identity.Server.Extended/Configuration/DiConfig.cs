using System.Reflection;
using Identity.Server.Extended.Services;
using Identity.Server.Extended.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerGenOptionDefaults();
            });
        builder.Services.AddEndpointsApiExplorer();
        return builder;
    }
    
    private static void SwaggerGenOptionDefaults(this SwaggerGenOptions options) {
        var version = $"v1";
        options.SwaggerDoc("identity", new OpenApiInfo {
            Version = version,
            Title = "Identity Server",
        });
        var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) {
            options.IncludeXmlComments(xmlPath);
        }
        options.OrderActionsBy(x => x.RelativePath);
        options.MapType<Stream>(() => new OpenApiSchema {
            Type = "string",
            Format = "binary"
        });
        options.CustomOperationIds(x => (x.ActionDescriptor as ControllerActionDescriptor)?.ActionName);
    }
}