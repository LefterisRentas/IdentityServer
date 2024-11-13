using System.Security.Claims;
using Identity.Server.Extended.Models;
using Identity.Server.Extended.Models.Clients;
using Identity.Server.Extended.Services.Abstractions;
using IdentityModel;
using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Services;

/// <summary>
/// <inheritdoc cref="IClientManager"/>
/// </summary>
public class ClientManager(IClientStore clientStore) : IClientManager
{
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
    public async Task<OperationResult<ClientsDto>> GetClientsAsync(string? search, int page = 1, int pageSize = 10)
    {
        return await clientStore.GetClientsAsync(search, page, pageSize);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientByIdAsync"/>
    /// </summary>
    public async Task<OperationResult<Client?>> GetClientByIdAsync(string clientId)
    {
        return await clientStore.GetClientByIdAsync(clientId);
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

        return await clientStore.CreateClientAsync(client, identity);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.UpdateClientAsync"/>
    /// </summary>
    public async Task<OperationResult<Client>> UpdateClientAsync(Client client, ClaimsPrincipal identity, bool updateClaims = false, bool updateProperties = false)
    {
        var validationResult = await ValidateClient(client, ClientAction.Update);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure<Client>(new Dictionary<string, List<string>> { { "validationErrors", validationResult.ValidationErrors.ToList() } });
        }

        return await clientStore.UpdateClientAsync(client, identity, updateClaims, updateProperties);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.DeleteClientAsync"/>
    /// </summary>
    public async Task<OperationResult> DeleteClientAsync(string clientId, ClaimsPrincipal identity)
    {
        // Optional: validate deletion if specific checks are needed
        var validationResult = await ValidateClient(new Client { ClientId = clientId }, ClientAction.Delete);
        if (!validationResult.IsValid)
        {
            return OperationResult.Failure(new Dictionary<string, List<string>> { { "validationErrors", validationResult.ValidationErrors.ToList() } });
        }

        return await clientStore.DeleteClientAsync(clientId, identity);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientSecretsAsync"/>
    /// </summary>
    public async Task<OperationResult<ClientSecretsDto>> GetClientSecretsAsync(int clientId, int pageSize = 10, int page = 1)
    {
        return await clientStore.GetClientSecretsAsync(clientId, pageSize, page);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.GetClientSecretAsync"/>
    /// </summary>
    public async Task<OperationResult<ClientSecretDto>> GetClientSecretAsync(int secretId)
    {
        return await clientStore.GetClientSecretAsync(secretId);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.AddClientSecretAsync"/>
    /// </summary>
    public async Task<OperationResult<ClientSecretDto>> AddClientSecretAsync(string clientId, ClientSecretDto clientSecretDto, ClaimsPrincipal identity)
    {
        return await clientStore.AddClientSecretAsync(clientId, clientSecretDto, identity);
    }

    /// <summary>
    /// <inheritdoc cref="IClientManager.RemoveClientSecretAsync"/>
    /// </summary>
    public async Task<OperationResult<ClientSecretDto>> RemoveClientSecretAsync(string clientId, int secretId, ClaimsPrincipal identity)
    {
        return await clientStore.RemoveClientSecretAsync(clientId, secretId, identity);
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
        var apiScopes = (await clientStore.GetApiScopesAsync()).Result?.Select(s => s.Name) ?? [];
        var identityResources = (await clientStore.GetIdentityResourcesAsync()).Result?.Select(s => s.Name) ?? [];

        // Combine the two lists into one list of all available scopes
        var allScopes = apiScopes.Concat(identityResources).ToList();
        var invalidScopes = client.AllowedScopes?.Where(scope => !allScopes.Contains(scope.Scope)).Select(x => x.Scope).ToList() ?? [];

        if (invalidScopes.Count != 0)
        {
            errors.Add($"The following scopes do not exist: {string.Join(", ", invalidScopes)}");
        }

        // Validate AllowedGrantTypes: check if each grant type is valid
        var validGrantTypes = (await clientStore.GetGrantTypesAsync()).Result?.Select(g => g.Type).ToList() ?? [];
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