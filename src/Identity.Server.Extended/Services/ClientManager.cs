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
    /// <inheritdoc cref="IClientManager.GetClientsAsync"/>
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Client>> GetClientsAsync()
    {
        var clients = _context.Clients.AsNoTracking().ToList();
        return Task.FromResult<IEnumerable<Client>>(clients);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientByIdAsync"/>
    /// </summary>
    public Task<Client?> GetClientByIdAsync(string clientId)
    {
        var client = _context.Clients.AsNoTracking().FirstOrDefault(c => c.ClientId == clientId);
        return Task.FromResult(client);
    }
}