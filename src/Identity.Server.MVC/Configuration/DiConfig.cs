using Identity.Server.MVC.Events.EventSinks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.MVC.Configuration;

public static class DiConfig
{
    public static WebApplicationBuilder AddDiConfig(this WebApplicationBuilder builder) {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        builder.Services.AddTransient<IEventSink, UserCreationEventSink>();
        return builder;
    }
}