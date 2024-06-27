using System;
using System.Reflection;
using Identity.Server.MVC.Data;
using Identity.Server.MVC.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.MVC.Configuration;

public static class IdentityServerConfig
{
    public static WebApplicationBuilder AddIdentityServerConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        }
        );

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var migrationsAssembly = typeof(IdentityServerConfig).GetTypeInfo().Assembly.GetName().Name;
        var identityServerBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
            options.EmitStaticAudienceClaim = true;
        })
            .AddOperationalStore(options =>
            {

                options.ConfigureDbContext = identityServerBuilder =>
                    identityServerBuilder.UseSqlServer(builder.Configuration.GetConnectionString("OperationalStoreConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));

                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = identityServerBuilder =>
                    identityServerBuilder.UseSqlServer(builder.Configuration.GetConnectionString("ConfigurationStoreConnection"),
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<ApplicationUser>();

        // not recommended for production - you need to store your key material somewhere secure
        AddDeveloperSigningCredential(identityServerBuilder);
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your Identity.Server.MVC with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:2000/signin-google
                options.ClientId = builder.Configuration.GetValue<string>("Google:ClientId");
                options.ClientSecret = builder.Configuration.GetValue<string>("Google:ClientSecret");
            });
        return builder;
    }
    
    [Obsolete]
    private static IIdentityServerBuilder AddDeveloperSigningCredential(IIdentityServerBuilder builder)
    {
        return builder.AddDeveloperSigningCredential();
    }
}