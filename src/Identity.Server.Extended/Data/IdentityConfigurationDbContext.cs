using Identity.Server.Extended.Data.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.Server.Extended.Data;

/// <summary>
/// The configuration context for the identity server.
/// </summary>
public class IdentityConfigurationDbContext : ConfigurationDbContext<IdentityConfigurationDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="IdentityConfigurationDbContext"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="storeOptions"></param>
    public IdentityConfigurationDbContext(
        DbContextOptions<IdentityConfigurationDbContext> options,
        ConfigurationStoreOptions storeOptions) : base(options, storeOptions)
    {
    }
    
    /// <summary>
    /// A table that contains the claim definitions.
    /// </summary>
    public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }
    public DbSet<GrantType> GrantTypes { get; set; }
}