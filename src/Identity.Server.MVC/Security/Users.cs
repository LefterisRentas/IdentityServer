using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Identity.Server.MVC.Data;
using Identity.Server.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Identity.Server.MVC.Security;

internal static class Users
{
    internal static async Task SeedUsersAndRoles(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                if (!context.Database.EnsureCreated())
                {
                    context.Database.Migrate();
                }
                
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRoles(roleMgr);
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                foreach (var testUser in TestUsers.Users)
                {
                    var user = await userMgr.FindByNameAsync(testUser.UserName ?? throw new InvalidOperationException("User name cannot be null at seed time"));
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = testUser.UserName,
                            Email = testUser.Claims.First(c => c.ToClaim().Type == JwtClaimTypes.Email).ToClaim().Value,
                            EmailConfirmed = true,
                            TwoFactorEnabled = testUser.TwoFactorEnabled,
                            Claims = testUser.Claims,
                            Id = testUser.Id
                        };

                        var result = await userMgr.CreateAsync(user, "Pass123$");
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = await userMgr.AddClaimsAsync(user, new Claim[]
                        {
                            new(JwtClaimTypes.Name, testUser.UserName),
                            new(JwtClaimTypes.GivenName, testUser.UserName),
                            new(JwtClaimTypes.FamilyName, testUser.UserName),
                            new(JwtClaimTypes.Email, testUser.Claims.First(c => c.ToClaim().Type == JwtClaimTypes.Email).ToClaim().Value),
                            new(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new(JwtClaimTypes.WebSite, "http://alice.com"),
                        });
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("User created: {username}", testUser.UserName);
                    }
                    else
                    {
                        Log.Debug("User already exists: {username}", testUser.UserName);
                    }
                }
            }
        }
    }
    
    internal static async Task SeedRoles(RoleManager<IdentityRole> roleMgr)
    {
        foreach (var role in Roles.GetRoles())
        {
            if (!await roleMgr.RoleExistsAsync(role.Name))
            {
                var result = await roleMgr.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }
}