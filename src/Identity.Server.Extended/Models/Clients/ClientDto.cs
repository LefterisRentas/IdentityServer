using Client = IdentityServer4.EntityFramework.Entities.Client;

namespace Identity.Server.Extended.Models.Clients;

public class ClientDto
{
    public static ClientDto FromEntity(Client client)
    {
        var clientDto = new ClientDto { 
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientUri = client.ClientUri,
                LogoUri = client.LogoUri,
                Description = client.Description,
                AllowRememberConsent = client.AllowRememberConsent,
                Enabled = client.Enabled,
                RequireConsent = client.RequireConsent,
                AllowedCorsOrigins = client.AllowedCorsOrigins.Select(x => x.Origin).ToList(),
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri).ToList(),
                RedirectUris = client.RedirectUris.Select(x => x.RedirectUri).ToList(),
                IdentityTokenLifetime = client.IdentityTokenLifetime,
                AccessTokenLifetime = client.AccessTokenLifetime,
                ConsentLifetime = client.ConsentLifetime,
                UserSsoLifetime = client.UserSsoLifetime,
                FrontChannelLogoutUri = client.FrontChannelLogoutUri,
                PairWiseSubjectSalt = client.PairWiseSubjectSalt,
                AccessTokenType = client.AccessTokenType,
                FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
                IncludeJwtId = client.IncludeJwtId,
                AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
                AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
                AlwaysSendClientClaims = client.AlwaysSendClientClaims,
                AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
                RequirePkce = client.RequirePkce,
                AllowPlainTextPkce = client.AllowPlainTextPkce,
                ClientClaimsPrefix = client.ClientClaimsPrefix,
                AllowedGrantTypes = client.AllowedGrantTypes.Select(x => x.GrantType).ToList(),
                AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
                AllowOfflineAccess = client.AllowOfflineAccess,
                NonEditable = client.NonEditable,
                RefreshTokenExpiration = client.RefreshTokenExpiration,
                RefreshTokenUsage = client.RefreshTokenUsage,
                UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
                BackChannelLogoutUri = client.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
                UserCodeType = client.UserCodeType,
                DeviceCodeLifetime = client.DeviceCodeLifetime,
                SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime,
                EnableLocalLogin = client.EnableLocalLogin,
                IdentityProviderRestrictions = client.IdentityProviderRestrictions.Select(x => x.Provider).ToList(),
                Claims = client.Claims.Select(x => new ClientClaimDto {
                    Id = x.Id,
                    Type = x.Type,
                    Value = x.Value
                }).ToList(),
                AllowedScopes = client.AllowedScopes.Select(x => x.Scope).ToList(),
                Updated = client.Updated,
                LastAccessed = client.LastAccessed,
                ProtocolType = client.ProtocolType,
                RequireClientSecret = client.RequireClientSecret,
                RequireRequestObject = client.RequireRequestObject,
                Id = client.Id,
                AllowedIdentityTokenSigningAlgorithms = client.AllowedIdentityTokenSigningAlgorithms.Split(',').ToList()
        };
        return clientDto;
    }

    public int AbsoluteRefreshTokenLifetime { get; set; } = (int)TimeSpan.FromDays(30).TotalSeconds;
    
    public int AccessTokenLifetime { get; set; } = (int)TimeSpan.FromHours(1).TotalSeconds;

    public int? ConsentLifetime { get; set; }

    public int AccessTokenType { get; set; }

    public bool AllowAccessTokensViaBrowser { get; set; }
    public bool AllowOfflineAccess { get; set; }
    public bool AllowPlainTextPkce { get; set; }
    public bool AllowRememberConsent { get; set; } = true;
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
    public bool AlwaysSendClientClaims { get; set; }
    public int AuthorizationCodeLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;

    public string FrontChannelLogoutUri { get; set; }
    public bool FrontChannelLogoutSessionRequired { get; set; } = true;
    public string BackChannelLogoutUri { get; set; }
    public bool BackChannelLogoutSessionRequired { get; set; } = true;

    public required string ClientId { get; set; }

    public required string ClientName { get; set; }

    public string ClientUri { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;
    public int Id { get; set; }
    public int IdentityTokenLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;
    public bool IncludeJwtId { get; set; }
    public string LogoUri { get; set; }

    public string ClientClaimsPrefix { get; set; } = "client_";

    public string PairWiseSubjectSalt { get; set; }

    public string ProtocolType { get; set; } = "oidc";


    public int RefreshTokenExpiration { get; set; } = 1;

    public int RefreshTokenUsage { get; set; } = 1;

    public int SlidingRefreshTokenLifetime { get; set; } = (int)TimeSpan.FromDays(15).TotalSeconds;

    public bool RequireClientSecret { get; set; } = true;
    public bool RequireConsent { get; set; } = true;
    public bool RequirePkce { get; set; }
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();

    public List<string> IdentityProviderRestrictions { get; set; } = new List<string>();

    public List<string> RedirectUris { get; set; } = new List<string>();

    public List<string> AllowedCorsOrigins { get; set; } = new List<string>();

    public List<string> AllowedGrantTypes { get; set; } = new List<string>();

    public List<string> AllowedScopes { get; set; } = new List<string>();

    public List<ClientClaimDto> Claims { get; set; } = new List<ClientClaimDto>();
    public List<ClientPropertyDto> Properties { get; set; } = new List<ClientPropertyDto>();

    public DateTime? Updated { get; set; }
    public DateTime? LastAccessed { get; set; }

    public int? UserSsoLifetime { get; set; }
    public string UserCodeType { get; set; }
    public int DeviceCodeLifetime { get; set; } = (int)TimeSpan.FromMinutes(5).TotalSeconds;

    public bool RequireRequestObject { get; set; }

    /// <summary>
    /// Specifies the algorithm used for signing the identity token.
    /// </summary>
    public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; } = new List<string>();

    /// <summary>
    /// Specifies if the client is non editable.
    /// </summary>
    public bool NonEditable { get; set; }
}