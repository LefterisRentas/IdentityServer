using System.Collections.Generic;
using IdentityModel;
using Identity.Server.MVC.Constants;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity.Server.MVC.Security;

public static class Clients
{
    public static IEnumerable<Client> ClientList { get; } =
        new List<Client>
        {
            // interactive ASP.NET Core MVC client
            new Client
            {
                ClientId = "mvc",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                // where to redirect to after login
                RedirectUris = { "https://localhost:2002/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:2002/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            
            // machine to machine client
            new Client
            {
                ClientId = "machine-client",
                ClientSecrets = { new Secret("B0665A0D-A0A0-461E-A6E8-F0C365C18B99".Sha256()) },

                AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.AuthorizationCode },
                
                AllowedScopes =
                {
                    ApiScopes.TestApi.Name, IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.StandardScopes.OpenId
                },
                
                AllowedCorsOrigins = { "https://localhost:2001" },
                
                RedirectUris = { "https://localhost:2001/docs/oauth2-redirect.html" },
                
                PostLogoutRedirectUris = { "https://localhost:2001/docs" },
                
                AllowOfflineAccess = true,
                
                AccessTokenLifetime = 300,
                
                AccessTokenType = AccessTokenType.Jwt,
                
                ClientClaimsPrefix = "",
                AlwaysSendClientClaims = true,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = true,
                Claims = { new ClientClaim(BasicClaimTypes.System, "true") }
            },

            new Client
            {
                ClientId = "swagger-ui",
                ClientSecrets = { new Secret("24B20CD5-0878-42DF-AC40-D45EE3E0E541".ToSha256()) },

                AllowedGrantTypes = { GrantType.ClientCredentials, GrantType.AuthorizationCode },

                AllowedScopes =
                [
                    ApiScopes.TestApi.Name, IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Phone,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Role
                ],

                AllowedCorsOrigins = { "https://localhost:2001" },

                // where to redirect to after login
                RedirectUris = { "https://localhost:2001/docs/oauth2-redirect.html" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:2001/docs" },

                AllowOfflineAccess = true,

                AccessTokenType = AccessTokenType.Reference,

                AuthorizationCodeLifetime = 300,

                Claims = { new ClientClaim("role", "application") }
            }
        };
}