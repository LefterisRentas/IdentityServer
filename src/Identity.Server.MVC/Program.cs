// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage();
}

app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.InitializeResourcesDatabase();
var seed = app.Services.GetService<IConfiguration>().GetValue<bool>("ShouldSeedDatabase");
if (seed)
{
    Log.Information("Seeding database...");
    var config = app.Services.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    SeedData.EnsureSeedData(connectionString);
    Log.Information("Done seeding database.");
}

Log.Information("Starting host...");
app.Run();