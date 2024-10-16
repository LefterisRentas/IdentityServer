﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Identity.Server.MVC.Security;

namespace Identity.Server.MVC.Data.Seeding;

public static class SeedData
{
    private const bool DELETE_DB = false;
    
    public static async Task EnsureSeedData(string connectionString)
    {
        await Users.SeedUsersAndRoles(connectionString);
    }

    internal static void InitializeResourcesDatabase(this IApplicationBuilder app)
    {
        SeedResources(app);
    }

    private static void SeedResources(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (serviceScope is null) return;
            var op = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                //This should not be used in a production environment
                if (DELETE_DB)
                {
                    op.Database.EnsureDeleted();
                }
                op.Database.EnsureCreated();
            }
            // op.Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                //This should not be used in a production environment
                if (DELETE_DB)
                {
                    context.Database.EnsureDeleted();
                }
                context.Database.EnsureCreated();
            }
            // context.Database.Migrate();

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in SeedingList.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in SeedingList.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in SeedingList.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                foreach (var client in SeedingList.Clients.ToList())
                {
                    context.Clients.AddRange(client.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}