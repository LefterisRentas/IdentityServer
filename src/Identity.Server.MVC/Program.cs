// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Identity.Server.Extended.Configuration;
using Identity.Server.Extended.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Identity.Server.MVC.Configuration;
using Identity.Server.MVC.Data.Seeding;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.AddIdentityServerConfig();
builder.AddDiConfig();
builder.AddExtendedIdentityServerDiConfig();

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

app.UseStaticFiles();

app.UseRouting();
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("Content-Security-Policy", "img-src 'self' data:;");
    await next();
});
app.UseIdentityServer();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.InitializeResourcesDatabase();
var seed = app.Services.GetService<IConfiguration>()?.GetValue<bool>("ShouldSeedDatabase") ?? false;
if (seed)
{
    Log.Information("Seeding database...");
    var config = app.Services.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    await SeedData.EnsureSeedData(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
    Log.Information("Done seeding database.");
}
app.MapSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.DocumentTitle = $"API Documentation";
    options.SwaggerEndpoint($"/swagger/identity/swagger.json", "Identity Server");
});
app.MapClients();
Log.Information("Starting host...");
app.Run();