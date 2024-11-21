// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Identity.Server.Extended.Configuration;
using Identity.Server.Extended.Endpoints;
using Identity.Server.Extended.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Identity.Server.MVC.Configuration;
using Identity.Server.MVC.Data.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
builder.AddDiConfig();
builder.AddExtendedIdentityServerDiConfig();
builder.AddExtendedIdentityServerAuthorizationConfig();
builder.AddIdentityServerConfig();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();
// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("Content-Security-Policy", "img-src 'self' https: data:;");
    await next();
});

// Authentication and Authorization
app.UseMiddleware<MultiSchemeAuthenticationMiddleware>();
app.UseIdentityServer(); // IdentityServer handles token validation and issuance
app.UseAuthorization(); // Handle authorization based on authenticated user

// Routes
app.MapDefaultControllerRoute(); // Map default routes for MVC
app.MapSwagger(); // Enable Swagger UI and documentation
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.DocumentTitle = "API Documentation";
    options.SwaggerEndpoint("/swagger/identity/swagger.json", "Identity Server");
});

app.MapApiResourcesManagement();
app.MapClientsManagement();
app.MapIdentityResourcesManagement();
app.MapRolesManagement();
app.MapScopesManagement();
app.MapUsersManagement();

var seed = app.Services.GetService<IConfiguration>()?.GetValue<bool>("ShouldSeedDatabase") ?? false;
if (seed)
{
    Log.Information("Seeding database...");
    app.InitializeResourcesDatabase();
    var config = app.Services.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    await SeedData.EnsureSeedData(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
    Log.Information("Done seeding database.");
}

Log.Information("Starting host...");
app.Run();