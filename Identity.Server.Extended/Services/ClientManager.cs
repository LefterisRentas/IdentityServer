using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Server.Extended.Services;

/// <summary>
/// <inheritdoc cref="IClientManager"/>
/// </summary>
public class ClientManager : IClientManager
{
    private readonly ConfigurationDbContext _context;
    
    /// <summary>
    /// <inheritdoc cref="IClientManager"/>
    /// </summary>
    public ClientManager(ConfigurationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// <inheritdoc cref="IClientManager"/>
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Client>> GetClientsAsync()
    {
        var clients = _context.Clients.AsNoTracking().ToList();
        return Task.FromResult<IEnumerable<Client>>(clients);
    }
}