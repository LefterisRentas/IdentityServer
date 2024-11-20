using System.Security.Claims;
using Identity.Server.Extended.Data.Entities;
using Identity.Server.Extended.Models;
using Identity.Server.Extended.Models.Clients;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Services.Abstractions;

/// <summary>
/// Interface for managing clients within the Identity Server, providing methods for CRUD operations,
/// secret management, and retrieval of related resources like grant types, API scopes, and identity resources.
/// </summary>
public interface IClientStore
{
    /// <summary>
    /// Retrieves all clients from the <see cref="ConfigurationDbContext"/> with optional search and pagination.
    /// </summary>
    /// <param name="search">Optional search term to filter clients by name or other criteria.</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>An <see cref="OperationResult{ClientsDto}"/> containing the list of clients with pagination details.</returns>
    Task<OperationResult<ClientsDto>> GetClientsAsync(string? search, int page = 1, int pageSize = 10);

    /// <summary>
    /// Retrieves a client by its unique identifier.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> containing the client details, or an error if not found.</returns>
    Task<OperationResult<Client?>> GetClientByIdAsync(string clientId);

    /// <summary>
    /// Creates a new client in the system.
    /// </summary>
    /// <param name="client">The client entity to create, including its configuration and settings.</param>
    /// <param name="identity">The user performing the creation, represented by their claims.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> indicating success or failure, and containing the created client if successful.</returns>
    Task<OperationResult<Client?>> CreateClientAsync(Client client, ClaimsPrincipal identity);

    /// <summary>
    /// Updates an existing client's details in the system, including optional updates to claims and properties.
    /// </summary>
    /// <param name="client">The client entity with updated information.</param>
    /// <param name="identity">The user performing the update, represented by their claims.</param>
    /// <param name="updateClaims">Indicates whether to update the client's claims.</param>
    /// <param name="updateProperties">Indicates whether to update the client's properties.</param>
    /// <returns>An <see cref="OperationResult{Client}"/> indicating the outcome of the update operation.</returns>
    Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity, bool updateClaims = false, bool updateProperties = false);

    /// <summary>
    /// Deletes a client by its unique identifier.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client to delete.</param>
    /// <param name="identity">The user performing the deletion, represented by their claims.</param>
    /// <returns>An <see cref="OperationResult"/> indicating the success or failure of the deletion operation.</returns>
    Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity);

    /// <summary>
    /// Retrieves a paginated list of secrets for a specific client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="pageSize">The number of secrets per page (default is 10).</param>
    /// <param name="page">The page number to retrieve (default is 1).</param>
    /// <returns>An <see cref="OperationResult{ClientSecretsDto}"/> containing the client secrets and pagination details.</returns>
    Task<OperationResult<ClientSecretsDto>> GetClientSecretsAsync(int clientId, int pageSize = 10, int page = 1);

    /// <summary>
    /// Retrieves a specific client secret by its unique identifier.
    /// </summary>
    /// <param name="secretId">The unique identifier of the secret.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> containing the client secret details, or an error if not found.</returns>
    Task<OperationResult<ClientSecretDto>> GetClientSecretAsync(int secretId);

    /// <summary>
    /// Adds a new secret to a specified client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="clientSecretDto">The details of the secret to add.</param>
    /// <param name="identity">The user performing the addition, represented by their claims.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> indicating the success or failure of the operation.</returns>
    Task<OperationResult<ClientSecretDto>> AddClientSecretAsync(int clientId, ClientSecretDto clientSecretDto, ClaimsPrincipal identity);

    /// <summary>
    /// Removes a specific secret from a client.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="secretId">The unique identifier of the secret to remove.</param>
    /// <param name="identity">The user performing the removal, represented by their claims.</param>
    /// <returns>An <see cref="OperationResult{ClientSecretDto}"/> indicating the outcome of the removal operation.</returns>
    Task<OperationResult<ClientSecretDto>> RemoveClientSecretAsync(int clientId, int secretId, ClaimsPrincipal identity);

    /// <summary>
    /// Retrieves all grant types available in the system.
    /// </summary>
    /// <returns>An <see cref="OperationResult"/> containing the list of grant types.</returns>
    Task<OperationResult<List<GrantType>>> GetGrantTypesAsync();

    /// <summary>
    /// Retrieves all API scopes defined in the system.
    /// </summary>
    /// <returns>An <see cref="OperationResult"/> containing the list of API scopes.</returns>
    Task<OperationResult<List<ApiScope>>> GetApiScopesAsync();

    /// <summary>
    /// Retrieves all identity resources defined in the system.
    /// </summary>
    /// <returns>An <see cref="OperationResult"/> containing the list of identity resources.</returns>
    Task<OperationResult<List<IdentityResource>>> GetIdentityResourcesAsync();
}
