using System.Security.Claims;
using Identity.Server.Extended.Models;
using Identity.Server.Extended.Models.Clients;
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
    /// If any entities like claims or properties are included in the client entity, they will be updated as well.
    /// </summary>
    /// <param name="client">The client entity with updated information, including client ID, name, and any modified settings.</param>
    /// <param name="identity">The user performing the update, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> indicating the outcome of the update operation, with the updated client entity if successful.</returns>
    Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity, bool updateClaims = false, bool updateProperties = false);

    /// <summary>
    /// Delete a client from the system.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client to be deleted.</param>
    /// <param name="identity">The user performing the deletion, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult"/> indicating success or failure of the deletion operation.</returns>
    Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity);
    
    /// <summary>
    /// Retrieves a paginated list of secrets associated with a specific client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client whose secrets are to be retrieved.</param>
    /// <param name="pageSize">The number of secrets to retrieve per page.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretsDto}"/> containing the list of client secrets, along with pagination details.</returns>
    Task<OperationResult<ClientSecretsDto>> GetClientSecretsAsync(int clientId, int pageSize = 10, int page = 1);

    /// <summary>
    /// Retrieves a specific client secret by its unique identifier.
    /// </summary>
    /// <param name="secretId">The unique identifier of the secret to retrieve.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> containing the requested client secret if found, or an error if it does not exist.</returns>
    Task<OperationResult<ClientSecretDto>> GetClientSecretAsync(int secretId);

    /// <summary>
    /// Adds a new secret to a specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client to which the secret will be added.</param>
    /// <param name="clientSecretDto">The details of the client secret to add, including values and expiration.</param>
    /// <param name="identity">The user adding the secret, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> indicating the outcome of the add operation, containing the newly added client secret if successful.</returns>
    Task<OperationResult<ClientSecretDto>> AddClientSecretAsync(string clientId, ClientSecretDto clientSecretDto, ClaimsPrincipal identity);

    /// <summary>
    /// Removes a specific secret from a client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client from which the secret will be removed.</param>
    /// <param name="secretId">The unique identifier of the secret to remove.</param>
    /// <param name="identity">The user performing the removal, represented by the current user's claims.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> indicating the success or failure of the removal operation.</returns>
    Task<OperationResult<ClientSecretDto>> RemoveClientSecretAsync(string clientId, int secretId, ClaimsPrincipal identity);
    
    // Task<ClientClaim> AddClientClaimAsync(string clientId, ClientClaimDto clientClaimDto, ClaimsPrincipal identity);
    //
    // Task<ClientClaim> UpdateClientClaimAsync(string clientId, ClientClaimDto clientClaimDto, ClaimsPrincipal identity);
    //
    // Task<ClientClaim> DeleteClientClaimAsync(string clientId, string claimType, ClaimsPrincipal identity);
}