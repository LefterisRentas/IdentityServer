using Identity.Server.Extended.Models;
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
    Task<OperationResult<IEnumerable<Client>?>> GetClientsAsync();
    
    /// <summary>
    /// Get a client by its id.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<OperationResult<Client?>> GetClientByIdAsync(string clientId);
    /// <summary>
    /// Create a new client.
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    Task<OperationResult<Client?>> CreateClientAsync(Client client);
    /// <summary>
    /// Update a client.
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    Task<OperationResult<Client>> UpdateClientAsync(Client client);
    /// <summary>
    /// Delete a client.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<OperationResult> DeleteClientAsync(string clientId);
}