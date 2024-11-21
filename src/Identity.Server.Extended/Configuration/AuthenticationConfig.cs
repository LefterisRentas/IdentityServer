using Duende.IdentityModel;
using Identity.Server.Extended.Helpers;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Configuration;

public static class AuthenticationConfig
{
    public static WebApplicationBuilder AddExtendedIdentityServerAuthenticationConfig(this WebApplicationBuilder builder) {
        var configuration = builder.Configuration;
        var authority = configuration.GetValue<string>("Authority")?.TrimEnd('/')
                        ?? throw new ArgumentNullException("Configuration key 'Authority' is missing.");

        builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                options.Authority = authority;
                options.MetadataAddress = $"{authority}/.well-known/openid-configuration";

                // Set claims mapping
                options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role; // "role"
                options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name; // "name"
                options.TokenValidationParameters.ValidateAudience = false;
                options.RequireHttpsMetadata = false;

                // Forward reference tokens to introspection
                options.ForwardDefaultSelector = BearerSelector.ForwardReferenceToken();
            })
            .AddOAuth2Introspection("introspection", options => {
                // Base address of the Identity Server
                options.Authority = authority;
                options.DiscoveryPolicy = new IdentityModel.Client.DiscoveryPolicy {
                    Authority = authority,
                    AdditionalEndpointBaseAddresses = new[] { authority },
                    ValidateIssuerName = false, // Consider enabling this for production
                    RequireHttps = false // Consider enabling HTTPS in production
                };

                // API client credentials for introspection
                var clientId = configuration.GetValue<string>("Introspection:ClientId")
                               ?? throw new ArgumentNullException("Configuration key 'Introspection:ClientId' is missing.");
                var clientSecret = configuration.GetValue<string>("Introspection:ClientSecret")
                                   ?? throw new ArgumentNullException("Configuration key 'Introspection:ClientSecret' is missing.");
                options.ClientId = clientId;
                options.ClientSecret = clientSecret;

                // Enable caching
                options.EnableCaching = true;
                options.CacheDuration = TimeSpan.FromMinutes(3); // Adjust based on application needs
            });
        return builder;
    }
}