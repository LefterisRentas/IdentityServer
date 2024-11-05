using System.Security.Claims;
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
    Task<OperationResult<IEnumerable<Client>?>> GetClientsAsync(string? search, int page = 1, int pageSize = 10);
    
    /// <summary>
    /// Get a client by its id.
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    Task<OperationResult<Client?>> GetClientByIdAsync(string clientId);

    /// <summary>
    /// Create a new client in the system.
    /// </summary>
    /// <param name="client">The client entity to create, containing client details such as client ID, name, and settings.</param>
    /// <param name="identity">The creator of the client, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> indicating success or failure, and containing the created client if successful.</returns>
    Task<OperationResult<Client?>> CreateClientAsync(Client client, ClaimsPrincipal identity);

    /// <summary>
    /// Update an existing client's details in the system.
    /// </summary>
    /// <param name="client">The client entity with updated information, including client ID, name, and any modified settings.</param>
    /// <param name="identity">The user performing the update, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> indicating the outcome of the update operation, with the updated client entity if successful.</returns>
    Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity);

    /// <summary>
    /// Delete a client from the system.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client to be deleted.</param>
    /// <param name="identity">The user performing the deletion, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult"/> indicating success or failure of the deletion operation.</returns>
    Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity);
}