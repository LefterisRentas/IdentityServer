using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Identity.Server.MVC.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Server.MVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //, IClientStore, IResourceStore
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // if (Debugger.IsAttached)
        // {
        //     if (!Database.EnsureCreated())
        //     {
        //         Database.Migrate();
        //     }
        // }
    }

    // public DbSet<Client> Clients { get; set; }
    // public DbSet<ApiResource> ApiResources { get; set; }
    // public DbSet<ApiScope> ApiScopes { get; set; }
    // public DbSet<IdentityResource> IdentityResources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //
    //     modelBuilder.Entity<ApiResource>(b =>
    //     {
    //         b.Property(u => u.Properties)
    //             .HasConversion(
    //                 d => JsonSerializer.Serialize(d, JsonSerializerOptions.Default),
    //                 s => JsonSerializer.Deserialize<IDictionary<string, string>>(s, JsonSerializerOptions.Default)
    //             );
    //     });
    // }
    //
    // public async Task<Client> FindClientByIdAsync(string clientId)
    // {
    //     return await Clients.FindAsync(clientId);
    // }
    //
    // public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    // {
    //     return await IdentityResources.Where(x => scopeNames.Contains(x.Name)).ToListAsync();
    // }
    //
    // public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    // {
    //     return await ApiScopes.Where(x => scopeNames.Contains(x.Name)).ToListAsync();
    // }
    //
    // public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    // {
    //     return await ApiResources.Where(x => x.Scopes.Any(s => scopeNames.Contains(s))).ToListAsync();
    // }
    //
    // public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    // {
    //     return await ApiResources.Where(x => apiResourceNames.Contains(x.Name)).ToListAsync();
    // }
    //
    // public async Task<Resources> GetAllResourcesAsync()
    // {
    //     var result = new Resources(IdentityResources, ApiResources, ApiScopes);
    //     return result;
    // }
}