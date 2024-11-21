using System.Collections.Generic;
using Identity.Server.Extended.Security;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity.Server.MVC.Security;

public static class Resources
{
    public static IEnumerable<ApiResource> GetApiResources() => new [] { TestApi, ExtendedIdentityServer };
    
    private static readonly ApiResource TestApi = new()
    {
        Name = ApiScopes.TestApi.Name,
        Description = ApiScopes.TestApi.Description,
        DisplayName = ApiScopes.TestApi.DisplayName,
        Scopes = [ApiScopes.TestApi.Name],
        ApiSecrets =
        {
            new Secret("5C6BDD7C-843B-4604-A55E-BC5201EA1E43".ToSha256())
        }
    };
    
    private static readonly ApiResource ExtendedIdentityServer = new()
    {
        Name = "ExtendedIdentityServer",
        Description = "Extended Identity Server",
        DisplayName = "Extended Identity Server",
        Scopes = [ApiScopes.OfflineAccess.Name],
        ApiSecrets =
        {
            new Secret("secret".ToSha256())
        }
    };
}