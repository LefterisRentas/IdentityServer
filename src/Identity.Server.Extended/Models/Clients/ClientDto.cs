using IdentityServer4.EntityFramework.Entities;

namespace Identity.Server.Extended.Models.Clients;

/// <summary>
/// Data Transfer Object for Identity Server client settings.
/// </summary>
public class ClientDto
{
    /// <summary>
    /// Creates a ClientDto from an IdentityServer4 Client entity.
    /// </summary>
    /// <param name="client">The IdentityServer4 Client entity.</param>
    /// <returns>A ClientDto populated with the client settings.</returns>
    public static ClientDto FromEntity(Client client)
    {
        var clientDto = new ClientDto
        {
            ClientId = client.ClientId,
            ClientName = client.ClientName
        };
        clientDto.ClientUri = client.ClientUri;
        clientDto.LogoUri = client.LogoUri;
        clientDto.Description = client.Description;
        clientDto.AllowRememberConsent = client.AllowRememberConsent;
        clientDto.Enabled = client.Enabled;
        clientDto.RequireConsent = client.RequireConsent;

        clientDto.AllowedCorsOrigins = client.AllowedCorsOrigins?.Select(x => x.Origin).ToList() ?? [];
        clientDto.PostLogoutRedirectUris = client.PostLogoutRedirectUris?.Select(x => x.PostLogoutRedirectUri).ToList() ?? [];
        clientDto.RedirectUris = client.RedirectUris?.Select(x => x.RedirectUri).ToList() ?? [];

        clientDto.IdentityTokenLifetime = client.IdentityTokenLifetime;
        clientDto.AccessTokenLifetime = client.AccessTokenLifetime;
        clientDto.ConsentLifetime = client.ConsentLifetime;
        clientDto.UserSsoLifetime = client.UserSsoLifetime;
        clientDto.FrontChannelLogoutUri = client.FrontChannelLogoutUri;
        clientDto.PairWiseSubjectSalt = client.PairWiseSubjectSalt;
        clientDto.AccessTokenType = client.AccessTokenType;
        clientDto.FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired;
        clientDto.IncludeJwtId = client.IncludeJwtId;
        clientDto.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
        clientDto.AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken;
        clientDto.AlwaysSendClientClaims = client.AlwaysSendClientClaims;
        clientDto.AuthorizationCodeLifetime = client.AuthorizationCodeLifetime;
        clientDto.RequirePkce = client.RequirePkce;
        clientDto.AllowPlainTextPkce = client.AllowPlainTextPkce;
        clientDto.ClientClaimsPrefix = client.ClientClaimsPrefix;

        clientDto.AllowedGrantTypes = client.AllowedGrantTypes?.Select(x => x.GrantType).ToList() ?? [];
        clientDto.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
        clientDto.AllowOfflineAccess = client.AllowOfflineAccess;
        clientDto.NonEditable = client.NonEditable;
        clientDto.RefreshTokenExpiration = client.RefreshTokenExpiration;
        clientDto.RefreshTokenUsage = client.RefreshTokenUsage;
        clientDto.UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh;
        clientDto.BackChannelLogoutUri = client.BackChannelLogoutUri;
        clientDto.BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired;
        clientDto.UserCodeType = client.UserCodeType;
        clientDto.DeviceCodeLifetime = client.DeviceCodeLifetime;
        clientDto.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
        clientDto.EnableLocalLogin = client.EnableLocalLogin;

        clientDto.IdentityProviderRestrictions = client.IdentityProviderRestrictions?.Select(x => x.Provider).ToList() ?? [];

        clientDto.Claims = client.Claims?.Select(x => new ClientClaimDto
        {
            Id = x.Id,
            Type = x.Type,
            Value = x.Value
        }).ToList() ?? [];

        clientDto.AllowedScopes = client.AllowedScopes?.Select(x => x.Scope).ToList() ?? [];

        clientDto.Updated = client.Updated;
        clientDto.LastAccessed = client.LastAccessed;
        clientDto.ProtocolType = client.ProtocolType;
        clientDto.RequireClientSecret = client.RequireClientSecret;
        clientDto.RequireRequestObject = client.RequireRequestObject;
        clientDto.Id = client.Id;

        clientDto.AllowedIdentityTokenSigningAlgorithms = !string.IsNullOrWhiteSpace(client.AllowedIdentityTokenSigningAlgorithms)
            ? client.AllowedIdentityTokenSigningAlgorithms.Split(',').ToList()
            : [];
        return clientDto;
    }

    /// <summary>Lifetime of the absolute refresh token in seconds (default: 30 days).</summary>
    public int AbsoluteRefreshTokenLifetime { get; set; } = (int)TimeSpan.FromDays(30).TotalSeconds;

    /// <summary>Lifetime of the access token in seconds (default: 1 hour).</summary>
    public int AccessTokenLifetime { get; set; } = (int)TimeSpan.FromHours(1).TotalSeconds;

    /// <summary>Lifetime of consent in seconds.</summary>
    public int? ConsentLifetime { get; set; }

    /// <summary>Type of access token (JWT or reference token).</summary>
    public int AccessTokenType { get; set; }

    /// <summary>Allows access tokens via the browser.</summary>
    public bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>Allows offline access (for refresh tokens).</summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>Allows plain text PKCE (Proof Key for Code Exchange).</summary>
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>Allows remembering the user consent.</summary>
    public bool AllowRememberConsent { get; set; } = true;

    /// <summary>Indicates if user claims should be included in the ID token.</summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>Indicates if client claims should always be sent.</summary>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>Lifetime of the authorization code in seconds (default: 5 minutes).</summary>
    public int AuthorizationCodeLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;

    /// <summary>Front-channel logout URI.</summary>
    public string FrontChannelLogoutUri { get; set; } = string.Empty;

    /// <summary>Indicates if front-channel logout session is required.</summary>
    public bool FrontChannelLogoutSessionRequired { get; set; } = true;

    /// <summary>Back-channel logout URI.</summary>
    public string BackChannelLogoutUri { get; set; } = string.Empty;

    /// <summary>Indicates if back-channel logout session is required.</summary>
    public bool BackChannelLogoutSessionRequired { get; set; } = true;

    /// <summary>Unique identifier for the client.</summary>
    public required string ClientId { get; set; }

    /// <summary>Name of the client application.</summary>
    public required string ClientName { get; set; }

    /// <summary>URI of the client application.</summary>
    public string ClientUri { get; set; } = string.Empty;

    /// <summary>Description of the client application.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Indicates if the client is enabled.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Enables local login for the client.</summary>
    public bool EnableLocalLogin { get; set; } = true;

    /// <summary>Database ID for the client.</summary>
    public int Id { get; set; }

    /// <summary>Lifetime of the identity token in seconds (default: 5 minutes).</summary>
    public int IdentityTokenLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;

    /// <summary>Indicates if a JWT ID should be included.</summary>
    public bool IncludeJwtId { get; set; }

    /// <summary>URI for the client's logo.</summary>
    public string LogoUri { get; set; } = string.Empty;

    /// <summary>Prefix for client claims.</summary>
    public string ClientClaimsPrefix { get; set; } = "client_";

    /// <summary>Salt used for pairwise subject creation.</summary>
    public string PairWiseSubjectSalt { get; set; } = string.Empty;

    /// <summary>Protocol type (e.g., "oidc").</summary>
    public string ProtocolType { get; set; } = "oidc";

    /// <summary>Specifies how the refresh token expiration is managed.</summary>
    public int RefreshTokenExpiration { get; set; } = 1;

    /// <summary>Specifies how the refresh token is used.</summary>
    public int RefreshTokenUsage { get; set; } = 1;

    /// <summary>Lifetime of the sliding refresh token in seconds (default: 15 days).</summary>
    public int SlidingRefreshTokenLifetime { get; set; } = (int)TimeSpan.FromDays(15).TotalSeconds;

    /// <summary>Specifies if the client secret is required.</summary>
    public bool RequireClientSecret { get; set; } = true;

    /// <summary>Indicates if consent is required for the client.</summary>
    public bool RequireConsent { get; set; } = true;

    /// <summary>Specifies if PKCE (Proof Key for Code Exchange) is required.</summary>
    public bool RequirePkce { get; set; }

    /// <summary>Indicates if access token claims should be updated on refresh.</summary>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>List of URIs for post-logout redirection.</summary>
    public List<string> PostLogoutRedirectUris { get; set; } = [];

    /// <summary>List of identity providers restricted for the client.</summary>
    public List<string> IdentityProviderRestrictions { get; set; } = [];

    /// <summary>List of URIs for redirection.</summary>
    public List<string> RedirectUris { get; set; } = [];

    /// <summary>List of allowed CORS origins.</summary>
    public List<string> AllowedCorsOrigins { get; set; } = [];

    /// <summary>List of allowed grant types for the client.</summary>
    public List<string> AllowedGrantTypes { get; set; } = [];

    /// <summary>List of allowed scopes for the client.</summary>
    public List<string> AllowedScopes { get; set; } = [];

    /// <summary>List of claims associated with the client.</summary>
    public List<ClientClaimDto> Claims { get; set; } = [];

    /// <summary>List of additional properties for the client.</summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<ClientPropertyDto> Properties { get; set; } = [];

    /// <summary>Last updated timestamp for the client.</summary>
    public DateTime? Updated { get; set; }

    /// <summary>Last accessed timestamp for the client.</summary>
    public DateTime? LastAccessed { get; set; }

    /// <summary>Specifies the single sign-on lifetime in seconds.</summary>
    public int? UserSsoLifetime { get; set; }

    /// <summary>Specifies the type of user code used by the client.</summary>
    public string UserCodeType { get; set; } = string.Empty;

    /// <summary>Lifetime of the device code in seconds (default: 5 minutes).</summary>
    public int DeviceCodeLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;

    /// <summary>Indicates if a request object is required.</summary>
    public bool RequireRequestObject { get; set; }

    /// <summary>List of algorithms allowed for signing identity tokens.</summary>
    public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = [];

    /// <summary>Indicates if the client is non-editable.</summary>
    public bool NonEditable { get; set; }
}