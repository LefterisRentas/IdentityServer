using Identity.Server.Extended.Models.Clients;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Client = IdentityServer4.EntityFramework.Entities.Client;
using ClientClaim = IdentityServer4.EntityFramework.Entities.ClientClaim;

namespace Identity.Server.Extended.Extensions;

/// <summary>
/// Mapping extensions for <see cref="Client"/> and <see cref="ClientDto"/>.
/// </summary>
public static class ClientEntityExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientDto"></param>
    /// <returns></returns>
    public static Client FromDto(ClientDto clientDto)
    {
        return new Client
        {
            ClientId = clientDto.ClientId,
            ClientName = clientDto.ClientName,
            ClientUri = clientDto.ClientUri,
            AllowedGrantTypes = clientDto.AllowedGrantTypes.Select(x => new ClientGrantType
            {
                GrantType = x
            }).ToList(),
            RequireClientSecret = clientDto.RequireClientSecret,
            AllowedScopes = clientDto.AllowedScopes.Select(x => new ClientScope
            {
                Scope = x
            }).ToList(),
            RedirectUris = clientDto.RedirectUris.Select(x => new ClientRedirectUri
            {
                RedirectUri = x
            }).ToList(),
            PostLogoutRedirectUris = clientDto.PostLogoutRedirectUris.Select(x => new ClientPostLogoutRedirectUri
            {
                PostLogoutRedirectUri = x
            }).ToList(),
            AllowedCorsOrigins = clientDto.AllowedCorsOrigins.Select(x => new ClientCorsOrigin
            {
                Origin = x
            }).ToList(),
            RequireConsent = clientDto.RequireConsent,
            AllowRememberConsent = clientDto.AllowRememberConsent,
            AlwaysIncludeUserClaimsInIdToken = clientDto.AlwaysIncludeUserClaimsInIdToken,
            RequirePkce = clientDto.RequirePkce,
            AllowPlainTextPkce = clientDto.AllowPlainTextPkce,
            RequireRequestObject = clientDto.RequireRequestObject,
            AllowOfflineAccess = clientDto.AllowOfflineAccess,
            UpdateAccessTokenClaimsOnRefresh = clientDto.UpdateAccessTokenClaimsOnRefresh,
            AccessTokenLifetime = clientDto.AccessTokenLifetime,
            IdentityTokenLifetime = clientDto.IdentityTokenLifetime,
            AuthorizationCodeLifetime = clientDto.AuthorizationCodeLifetime,
            AbsoluteRefreshTokenLifetime = clientDto.AbsoluteRefreshTokenLifetime,
            SlidingRefreshTokenLifetime = clientDto.SlidingRefreshTokenLifetime,
            RefreshTokenUsage = clientDto.RefreshTokenUsage,
            RefreshTokenExpiration = clientDto.RefreshTokenExpiration,
            ConsentLifetime = clientDto.ConsentLifetime,
            FrontChannelLogoutUri = clientDto.FrontChannelLogoutUri,
            FrontChannelLogoutSessionRequired = clientDto.FrontChannelLogoutSessionRequired,
            BackChannelLogoutUri = clientDto.BackChannelLogoutUri,
            BackChannelLogoutSessionRequired = clientDto.BackChannelLogoutSessionRequired,
            AllowAccessTokensViaBrowser = clientDto.AllowAccessTokensViaBrowser,
            EnableLocalLogin = clientDto.EnableLocalLogin,
            IncludeJwtId = clientDto.IncludeJwtId,
            AlwaysSendClientClaims = clientDto.AlwaysSendClientClaims,
            ClientClaimsPrefix = clientDto.ClientClaimsPrefix,
            PairWiseSubjectSalt = clientDto.PairWiseSubjectSalt,
            IdentityProviderRestrictions = clientDto.IdentityProviderRestrictions.Select(x => new ClientIdPRestriction
            {
                Provider = x
            }).ToList(),
            Claims = clientDto.Claims.Select(x => x.ToEntity(clientDto.Id)).ToList(),
            AllowedIdentityTokenSigningAlgorithms = string.Join(",", clientDto.AllowedIdentityTokenSigningAlgorithms),
            UserSsoLifetime = clientDto.UserSsoLifetime,
            UserCodeType = clientDto.UserCodeType,
            DeviceCodeLifetime = clientDto.DeviceCodeLifetime,
            NonEditable = clientDto.NonEditable,
            Description = clientDto.Description,
            // Casting first to enum then to int to avoid data corruption.
            AccessTokenType = (int)(AccessTokenType)clientDto.AccessTokenType,
            LogoUri = clientDto.LogoUri,
            ProtocolType = clientDto.ProtocolType,
            Enabled = clientDto.Enabled,
            LastAccessed = clientDto.LastAccessed,
            Properties = clientDto.Properties.Select(x => x.ToEntity(clientDto.Id)).ToList(),
            Updated = clientDto.Updated,
            Id = clientDto.Id
        };
    } 
}