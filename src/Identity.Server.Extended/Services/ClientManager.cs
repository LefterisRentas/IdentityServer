using System.Security.Claims;
using Identity.Server.Extended.Data;
using Identity.Server.Extended.Events;
using Identity.Server.Extended.Models;
using Identity.Server.Extended.Services.Abstractions;
using IdentityModel;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;

namespace Identity.Server.Extended.Services;

/// <summary>
/// <inheritdoc cref="IClientManager"/>
/// </summary>
public class ClientManager<TConfigurationDbContext>(TConfigurationDbContext context, IEventService events) : IClientManager where TConfigurationDbContext : IdentityConfigurationDbContext
{
    private readonly TConfigurationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IEventService _events = events ?? throw new ArgumentNullException(nameof(events));
    private static readonly List<string> StandardIdentityServerGrantTypes =
    [
        OidcConstants.GrantTypes.AuthorizationCode,
        OidcConstants.GrantTypes.ClientCredentials,
        OidcConstants.GrantTypes.Implicit,
        OidcConstants.GrantTypes.Password,
        OidcConstants.GrantTypes.DeviceCode,
        OidcConstants.GrantTypes.RefreshToken,
        OidcConstants.GrantTypes.JwtBearer,
        OidcConstants.GrantTypes.Ciba,
        OidcConstants.GrantTypes.Saml2Bearer,
        OidcConstants.GrantTypes.TokenExchange
    ];

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientsAsync"/>
    /// </summary>
    /// <returns></returns>
    public Task<OperationResult<IEnumerable<Client>?>> GetClientsAsync(string? search, int page = 1, int pageSize = 10)
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
            .Where(x => string.IsNullOrWhiteSpace(search) || x.ClientId.Contains(search) || x.ClientName.Contains(search))
            .Skip((page - 1) * pageSize)
            .Take(pageSize).AsEnumerable();
        return Task.FromResult(OperationResult.Success(clients));
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientByIdAsync"/>
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
    /// <inheritdoc cref="IClientManager.CreateClientAsync"/>
    /// </summary>
    public async Task<OperationResult<Client?>> CreateClientAsync(Client client, ClaimsPrincipal identity)
    {
        var validationResult = await ValidateClient(client, ClientAction.Create);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure<Client?>(new Dictionary<string, List<string>> { { "validationErrors", validationResult.ValidationErrors.ToList() } });
        }

        if (_context.Clients.Any(c => c.ClientId == client.ClientId))
        {
            return OperationResult.Failure<Client?>(new Dictionary<string, List<string>> { { "ClientId", ["ClientId Already Exists."] } });
        }

        client.Id = 0;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        await _events.RaiseAsync(new ClientCreationEvent(identity) { ClientId = client.ClientId, ClientName = client.ClientName });
        return OperationResult.Success(client);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.UpdateClientAsync"/>
    /// </summary>
    public async Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity)
    {
        var validationResult = await ValidateClient(client, ClientAction.Update);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure<Client>(new Dictionary<string, List<string>> { { "validationErrors", validationResult.ValidationErrors.ToList() } });;
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.DeleteClientAsync"/>
    /// </summary>
    public async Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity)
    {
        var validationResult = await ValidateClient(new Client { ClientId = clientId }, ClientAction.Delete);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure(new Dictionary<string, List<string>> { { "validationErrors", validationResult.ValidationErrors.ToList() } });
        }

        var client = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);
        if (client is null)
        {
            return OperationResult.Failure(new Dictionary<string, List<string>> { { "ClientId", ["Client not found."] } });
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return OperationResult.Success();
    }

    private async Task<ValidationResult> ValidateClient(Client client, ClientAction action)
    {
        var errors = new List<string>();
        switch (action)
        {
            case ClientAction.Create:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }

                if (string.IsNullOrWhiteSpace(client.ClientName))
                {
                    errors.Add("Client Name is required.");
                }

                break;
            }
            case ClientAction.Update:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }

                if (string.IsNullOrWhiteSpace(client.ClientName))
                {
                    errors.Add("Client Name is required.");
                }

                break;
            }
            case ClientAction.Delete:
            {
                if (string.IsNullOrWhiteSpace(client.ClientId))
                {
                    errors.Add("Client Id is required.");
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }

        // Validate AllowedScopes: check if each requested scope exists
        // Retrieve API scopes and identity resource scopes, then combine them
        var apiScopes = await _context.ApiScopes.Select(s => s.Name).ToListAsync();
        var identityResources = await _context.IdentityResources.Select(r => r.Name).ToListAsync();

        // Combine the two lists into one list of all available scopes
        var allScopes = apiScopes.Concat(identityResources).ToList();
        var invalidScopes = client.AllowedScopes?.Where(scope => !allScopes.Contains(scope.Scope)).Select(x => x.Scope).ToList() ?? [];

        if (invalidScopes.Count != 0)
        {
            errors.Add($"The following scopes do not exist: {string.Join(", ", invalidScopes)}");
        }

        // Validate AllowedGrantTypes: check if each grant type is valid
        var validGrantTypes = _context.GrantTypes.Select(g => g.Type).ToList();
        if (validGrantTypes.Count == 0)
        {
            validGrantTypes = StandardIdentityServerGrantTypes;
        }

        var invalidGrantTypes = client.AllowedGrantTypes?.Where(grantType => !validGrantTypes.Contains(grantType.GrantType)).Select(x => x.GrantType).ToList() ?? [];

        if (invalidGrantTypes.Count != 0)
        {
            errors.Add($"The following grant types are invalid: {string.Join(", ", invalidGrantTypes)}.");
        }

        // Validate RedirectUris are well-formed
        if (client.RedirectUris != null)
        {
            foreach (var uri in client.RedirectUris)
            {
                if (!Uri.IsWellFormedUriString(uri.RedirectUri, UriKind.Absolute))
                {
                    errors.Add($"Invalid Redirect URI: {uri.RedirectUri}");
                }
            }
        }

        // Validate PostLogoutRedirectUris are well-formed
        if (client.PostLogoutRedirectUris != null)
        {
            foreach (var uri in client.PostLogoutRedirectUris)
            {
                if (!Uri.IsWellFormedUriString(uri.PostLogoutRedirectUri, UriKind.Absolute))
                {
                    errors.Add($"Invalid Post Logout Redirect URI: {uri.PostLogoutRedirectUri}");
                }
            }
        }

        // Validate token lifetimes are within a reasonable range
        if (client.AccessTokenLifetime is < 300 or > 86400)
        {
            errors.Add("Access Token Lifetime must be between 5 minutes and 24 hours.");
        }

        if (client.IdentityTokenLifetime is < 300 or > 86400)
        {
            errors.Add("Identity Token Lifetime must be between 5 minutes and 24 hours.");
        }

        if (client.AuthorizationCodeLifetime is < 300 or > 86400)
        {
            errors.Add("Authorization Code Lifetime must be between 5 minutes and 24 hours.");
        }

        if (client.AbsoluteRefreshTokenLifetime is < 300 or > 2592000)
        {
            errors.Add("Absolute Refresh Token Lifetime must be between 5 minutes and 30 days.");
        }

        if (client.SlidingRefreshTokenLifetime is < 300 or > 2592000)
        {
            errors.Add("Sliding Refresh Token Lifetime must be between 5 minutes and 30 days.");
        }

        return new ValidationResult(errors.Count == 0, errors.ToArray());
    }

    private enum ClientAction
    {
        Create,
        Update,
        Delete
    }
}