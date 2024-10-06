using System.Collections.Generic;
using Identity.Server.Extended.Security;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity.Server.MVC.Security;

public static class Resources
{
    public static IEnumerable<ApiResource> GetApiResources() => new [] { ErpApi };
    
    private static readonly ApiResource ErpApi = new ApiResource()
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
}