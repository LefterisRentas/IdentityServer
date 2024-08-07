// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
app.UseIdentityServer();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.InitializeResourcesDatabase();
var seed = app.Services.GetService<IConfiguration>().GetValue<bool>("ShouldSeedDatabase");
if (seed)
{
    Log.Information("Seeding database...");
    var config = app.Services.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    await SeedData.EnsureSeedData(connectionString);
    Log.Information("Done seeding database.");
}

app.MapClients();
Log.Information("Starting host...");
app.Run();