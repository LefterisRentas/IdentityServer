using System.Security.Claims;
using Identity.Server.Extended.Data;
using Identity.Server.Extended.Data.Entities;
using Identity.Server.Extended.Events;
using Identity.Server.Extended.Extensions;
using Identity.Server.Extended.Models;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Identity.Server.Extended.Services;

internal class ClientStore<TConfigurationDbContext>(
    TConfigurationDbContext context,
    IEventService events,
    ILogger<ClientStore<TConfigurationDbContext>> logger) : IClientStore where TConfigurationDbContext : IdentityConfigurationDbContext
{
    private readonly TConfigurationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IEventService _events = events ?? throw new ArgumentNullException(nameof(events));
    private readonly ILogger<ClientStore<TConfigurationDbContext>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    
    /// <summary>
    /// <inheritdoc cref="IClientStore.GetClientsAsync"/>
    /// </summary>
    /// <returns></returns>
    public Task<OperationResult<ClientsDto>> GetClientsAsync(string? search, int page = 1, int pageSize = 10)
    {
        var clients = _context.Clients.AsNoTracking()
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties)
            .Where(x => string.IsNullOrWhiteSpace(search) || x.ClientId.Contains(search) || x.ClientName.Contains(search)).ToList();
        
        return Task.FromResult(OperationResult.Success(ClientsDto.FromEntities(clients, pageSize, page)))!;
    }
    
    /// <summary>
    /// <inheritdoc cref="IClientStore.GetClientByIdAsync"/>
    /// </summary>
    public Task<OperationResult<Client?>> GetClientByIdAsync(string clientId)
    {
        var client = _context.Clients.AsNoTracking()
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties)
            .FirstOrDefault(c => c.ClientId == clientId);
        return Task.FromResult(client is null ? OperationResult.Failure<Client?>(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } }) : OperationResult.Success(client));
    }
    
    /// <summary>
    /// <inheritdoc cref="IClientStore.CreateClientAsync"/>
    /// </summary>
    public async Task<OperationResult<Client?>> CreateClientAsync(Client client, ClaimsPrincipal identity)
    {
        if (_context.Clients.Any(c => c.ClientId == client.ClientId))
        {
            return OperationResult.Failure<Client?>(new Dictionary<string, List<string>> { { "ClientId", ["ClientId Already Exists."] } });
        }

        client.Id = 0;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        await _events.RaiseAsync(new ClientCreationEvent(identity, client.ClientId, client.ClientName));
        return OperationResult.Success(client);
    }
    
        /// <summary>
    /// <inheritdoc cref="IClientStore.UpdateClientAsync"/>
    /// </summary>
    public async Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity, bool updateClaims = false, bool updateProperties = false)
    {
        try
        {
            // Retrieve the existing client from the database
            var existingClient = _context.Clients.Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .FirstOrDefault(c => c.ClientId == client.ClientId);

            if (existingClient == null)
            {
                return OperationResult.Failure<Client>(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } });
            }

            // Deep copy of the original client
            var originalClientAsDto = ClientDto.FromEntity(existingClient);
            var originalClient = ClientEntityExtensions.FromDto(originalClientAsDto);
            // Update main client fields
            UpdateClientFields(existingClient, client);

            // Update claims if specified
            if (updateClaims)
            {
                UpdateClientClaims(existingClient, client);
            }

            // Update properties if specified
            if (updateProperties)
            {
                UpdateClientProperties(existingClient, client);
            }

            // Update other collections: AllowedScopes, RedirectUris, etc.
            UpdateClientCollections(existingClient, client);

            // Save changes to the database
            await _context.SaveChangesAsync();

            await _events.RaiseAsync(new ClientUpdateEvent(identity, originalClient, existingClient));
            
            // Return success result with the updated client
            return OperationResult.Success(existingClient)!; //Existing client is not null
        }
        catch (Exception ex)
        {
            // Log exception and return failure with error message
            _logger.LogError(ex, "An error occurred while updating the client.");
            return OperationResult.Failure<Client>(new Dictionary<string, List<string>> { { "error", ["An error occurred while updating the client."] } });
        }
    }
        
    /// <summary>
    /// <inheritdoc cref="IClientStore.DeleteClientAsync"/>
    /// </summary>
    public async Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity)
    {
        try
        {
            // Retrieve the client with related entities
            var client = await _context.Clients
                .Include(c => c.AllowedGrantTypes)
                .Include(c => c.RedirectUris)
                .Include(c => c.PostLogoutRedirectUris)
                .Include(c => c.AllowedScopes)
                .Include(c => c.ClientSecrets)
                .Include(c => c.Claims)
                .Include(c => c.IdentityProviderRestrictions)
                .Include(c => c.AllowedCorsOrigins)
                .Include(c => c.Properties)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (client is null)
            {
                return OperationResult.Failure(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } });
            }
            
            var clientDto = ClientDto.FromEntity(client);

            // Remove the client and all related entities
            _context.Clients.Remove(client);

            // Save changes to delete the client and its related entities
            await _context.SaveChangesAsync();

            await events.RaiseAsync(new ClientDeletionEvent(clientDto, identity) { ClientId = clientId, ClientName = client.ClientName });
            
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            // Log the exception and return a failure result
            _logger.LogError(ex, "An error occurred while deleting the client.");
            return OperationResult.Failure(new Dictionary<string, List<string>> { { "error", ["An error occurred while deleting the client."] } });
        }
    }
    
    /// <summary>
    /// <inheritdoc cref="IClientStore.GetClientSecretsAsync"/>
    /// </summary>
    public Task<OperationResult<ClientSecretsDto>> GetClientSecretsAsync(int clientId, int pageSize = 10, int page = 1)
    {
        var clientSecrets = _context.ClientSecrets.AsNoTracking()
            .Where(x => x.ClientId == clientId)
            .ToList();
        
        return Task.FromResult(OperationResult.Success(ClientSecretsDto.FromEntities(clientSecrets, pageSize, page)))!; //ClientSecretsDto.FromEntities will not return null
    }
    
        /// <summary>
    /// <inheritdoc cref="IClientStore.GetClientSecretAsync"/>
    /// </summary>
    public Task<OperationResult<ClientSecretDto>> GetClientSecretAsync(int secretId)
    {
        var clientSecret = _context.ClientSecrets.AsNoTracking().FirstOrDefault(x => x.Id == secretId);
        return Task.FromResult(clientSecret is null ? 
            OperationResult.Failure<ClientSecretDto>(new Dictionary<string, List<string>> { { "SecretId", ["Client Secret not found."] } })
            : OperationResult.Success(ClientSecretDto.FromEntity(clientSecret))!);
    }

    /// <summary>
    /// <inheritdoc cref="IClientStore.AddClientSecretAsync"/>
    /// </summary>
    public Task<OperationResult<ClientSecretDto>> AddClientSecretAsync(int clientId, ClientSecretDto clientSecretDto, ClaimsPrincipal identity)
    {
        var client = _context.Clients.FirstOrDefault(x => x.Id == clientId);
        if (client is null)
        {
            return Task.FromResult(OperationResult.Failure<ClientSecretDto>(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } }));
        }

        var clientSecret = clientSecretDto.ToEntity(client.Id);

        _context.ClientSecrets.Add(clientSecret);
        _context.SaveChanges();

        return Task.FromResult(OperationResult.Success(ClientSecretDto.FromEntity(clientSecret)))!;
    }

    /// <summary>
    /// <inheritdoc cref="IClientStore.RemoveClientSecretAsync"/>
    /// </summary>
    public Task<OperationResult<ClientSecretDto>> RemoveClientSecretAsync(int clientId, int secretId, ClaimsPrincipal identity)
    {
        var client = _context.Clients.FirstOrDefault(x => x.Id == clientId);
        if (client is null)
        {
            return Task.FromResult(OperationResult.Failure<ClientSecretDto>(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } }));
        }

        var clientSecret = _context.ClientSecrets.FirstOrDefault(x => x.Id == secretId);
        if (clientSecret is null)
        {
            return Task.FromResult(OperationResult.Failure<ClientSecretDto>(new Dictionary<string, List<string>> { { "SecretId", ["Client Secret not found."] } }));
        }

        _context.ClientSecrets.Remove(clientSecret);
        _context.SaveChanges();

        return Task.FromResult(OperationResult.Success(ClientSecretDto.FromEntity(clientSecret)))!;
    }

    public async Task<OperationResult<List<GrantType>>> GetGrantTypesAsync()
    {
        var grantTypes = await _context.GrantTypes.AsNoTracking().ToListAsync();
        return OperationResult.Success(grantTypes)!;
    }

    public Task<OperationResult<List<ApiScope>>> GetApiScopesAsync()
    {
        var apiScopes = _context.ApiScopes.AsNoTracking().ToList();
        return Task.FromResult(OperationResult.Success(apiScopes))!;
    }

    public Task<OperationResult<List<IdentityResource>>> GetIdentityResourcesAsync()
    {
        var identityResources = _context.IdentityResources.AsNoTracking().ToList();
        return Task.FromResult(OperationResult.Success(identityResources))!;
    }

    // Helper method to update main client fields
    private void UpdateClientFields(Client existingClient, Client client)
    {
        existingClient.ClientName = client.ClientName;
        existingClient.Updated = DateTime.UtcNow;
        existingClient.Description = client.Description;
        existingClient.ClientUri = client.ClientUri;
        existingClient.LogoUri = client.LogoUri;
        existingClient.Enabled = client.Enabled;
        existingClient.RequireConsent = client.RequireConsent;
        existingClient.AllowRememberConsent = client.AllowRememberConsent;
        existingClient.IdentityTokenLifetime = client.IdentityTokenLifetime;
        existingClient.AccessTokenLifetime = client.AccessTokenLifetime;
        existingClient.AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
        existingClient.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
        existingClient.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
        existingClient.ConsentLifetime = client.ConsentLifetime;
        existingClient.RefreshTokenExpiration = client.RefreshTokenExpiration;
        existingClient.RefreshTokenUsage = client.RefreshTokenUsage;
        existingClient.IncludeJwtId = client.IncludeJwtId;
        existingClient.AlwaysSendClientClaims = client.AlwaysSendClientClaims;
        existingClient.AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
        existingClient.AllowOfflineAccess = client.AllowOfflineAccess;
        existingClient.UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;
        existingClient.BackChannelLogoutUri = client.BackChannelLogoutUri;
        existingClient.BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired;
        existingClient.FrontChannelLogoutUri = client.FrontChannelLogoutUri;
        existingClient.FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired;
        existingClient.ClientClaimsPrefix = client.ClientClaimsPrefix;
        existingClient.PairWiseSubjectSalt = client.PairWiseSubjectSalt;
        existingClient.UserSsoLifetime = client.UserSsoLifetime;
        existingClient.ProtocolType = client.ProtocolType;
        existingClient.RequireClientSecret = client.RequireClientSecret;
        existingClient.RequireRequestObject = client.RequireRequestObject;
        existingClient.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
        existingClient.AllowedIdentityTokenSigningAlgorithms = client.AllowedIdentityTokenSigningAlgorithms;
    }

    // Helper method to update claims
    private void UpdateClientClaims(Client existingClient, Client client)
    {
        _context.ClientClaims.RemoveRange(existingClient.Claims.Where(c => client.Claims.All(x => x.Type != c.Type)));

        _context.ClientClaims.AddRange(client.Claims.Where(c => existingClient.Claims.All(x => x.Type != c.Type))
            .Select(claim => new ClientClaim
            {
                ClientId = existingClient.Id,
                Type = claim.Type,
                Value = claim.Value
            }));

        foreach (var existingClaim in existingClient.Claims)
        {
            var updatedClaim = client.Claims.FirstOrDefault(c => c.Type == existingClaim.Type);
            if (updatedClaim != null && existingClaim.Value != updatedClaim.Value)
            {
                existingClaim.Value = updatedClaim.Value;
                _context.ClientClaims.Update(existingClaim);
            }
        }
    }

    // Helper method to update properties
    private void UpdateClientProperties(Client existingClient, Client client)
    {
        _context.ClientProperties.RemoveRange(existingClient.Properties.Where(p => client.Properties.All(x => x.Key != p.Key)));

        _context.ClientProperties.AddRange(client.Properties.Where(p => existingClient.Properties.All(x => x.Key != p.Key))
            .Select(property => new ClientProperty
            {
                ClientId = existingClient.Id,
                Key = property.Key,
                Value = property.Value
            }));

        foreach (var existingProperty in existingClient.Properties)
        {
            var updatedProperty = client.Properties.FirstOrDefault(p => p.Key == existingProperty.Key);
            if (updatedProperty != null && existingProperty.Value != updatedProperty.Value)
            {
                existingProperty.Value = updatedProperty.Value;
                _context.ClientProperties.Update(existingProperty);
            }
        }
    }

    // Helper method to update collections like AllowedScopes, RedirectUris, etc.
    private void UpdateClientCollections(Client existingClient, Client client)
    {
        existingClient.AllowedScopes = SynchronizeCollections(existingClient.AllowedScopes, client.AllowedScopes, x => x.Scope);
        existingClient.AllowedGrantTypes = SynchronizeCollections(existingClient.AllowedGrantTypes, client.AllowedGrantTypes, x => x.GrantType);
        existingClient.RedirectUris = SynchronizeCollections(existingClient.RedirectUris, client.RedirectUris, x => x.RedirectUri);
        existingClient.PostLogoutRedirectUris = SynchronizeCollections(existingClient.PostLogoutRedirectUris, client.PostLogoutRedirectUris, x => x.PostLogoutRedirectUri);
        existingClient.IdentityProviderRestrictions = SynchronizeCollections(existingClient.IdentityProviderRestrictions, client.IdentityProviderRestrictions, x => x.Provider);
        existingClient.AllowedCorsOrigins = SynchronizeCollections(existingClient.AllowedCorsOrigins, client.AllowedCorsOrigins, x => x.Origin);
    }

    // Generic method to synchronize collections based on a key selector
    private List<T> SynchronizeCollections<T>(List<T> existing, List<T> updated, Func<T, string> keySelector) where T : class, new()
    {
        var toRemove = existing.Where(e => updated.All(u => keySelector(u) != keySelector(e))).ToList();
        var toAdd = updated.Where(u => existing.All(e => keySelector(e) != keySelector(u))).ToList();

        _context.RemoveRange(toRemove);
        _context.AddRange(toAdd);

        return existing.Except(toRemove).Concat(toAdd).ToList();
    }
}