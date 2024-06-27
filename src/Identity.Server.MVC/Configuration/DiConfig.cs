using Microsoft.AspNetCore.Builder;

namespace Identity.Server.MVC.Configuration;

public static class DiConfig
{
    public static WebApplicationBuilder AddDiConfig(this WebApplicationBuilder builder) {
        return builder;
    }
}