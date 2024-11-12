using Identity.Server.Extended.Data.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.Server.Extended.Data;

/// <summary>
/// The configuration context for the identity server.
/// </summary>
public class IdentityConfigurationDbContext : ConfigurationDbContext<IdentityConfigurationDbContext>
{
    
    public DbSet<ClientClaim> ClientClaims { get; set; }
    
    public DbSet<ClientProperty> ClientProperties { get; set; }
    
    public DbSet<ClientSecret> ClientSecrets { get; set; }
    
    public DbSet<ClientScope> ClientScopes { get; set; }
    
    /// <summary>
    /// A table that contains the claim definitions.
    /// </summary>
    public DbSet<ClaimDefinition> ClaimDefinitions { get; set; }
    public DbSet<GrantType> GrantTypes { get; set; }
    
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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure ClientClaim entity
        modelBuilder.Entity<ClientClaim>(claim =>
        {
            claim.ToTable("ClientClaims"); // claim.ToTable(storeOptions.ClientClaim);
            claim.Property(x => x.Type).HasMaxLength(250).IsRequired();
            claim.Property(x => x.Value).HasMaxLength(250).IsRequired();
        });

        // Configure ClientProperty entity
        modelBuilder.Entity<ClientProperty>(property =>
        {
            property.ToTable("ClientProperties"); // property.ToTable(storeOptions.ClientProperty);
            property.Property(x => x.Key).HasMaxLength(250).IsRequired();
            property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
        });
        
        modelBuilder.Entity<ClientSecret>(secret =>
        {
            secret.ToTable("ClientSecrets"); // secret.ToTable(storeOptions.ClientSecret);
            secret.Property(x => x.Value).HasMaxLength(4000).IsRequired();
            secret.Property(x => x.Type).HasMaxLength(250).IsRequired();
            secret.Property(x => x.Description).HasMaxLength(2000);
        });
        
        modelBuilder.Entity<ClientScope>(scope =>
        {
            scope.ToTable("ClientScopes"); // scope.ToTable(storeOptions.ClientScope);
            scope.Property(x => x.Scope).HasMaxLength(200).IsRequired();
        });
    }
}