using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Services.Abstractions;

/// <summary>
/// The client manager.
/// </summary>
public interface IClientManager
{
    /// <summary>
    /// Get all clients from the <see cref="ConfigurationDbContext"/>
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Client>> GetClientsAsync();
    
    /// <summary>
    /// Get a client by its id.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<Client?> GetClientByIdAsync(string clientId);
}