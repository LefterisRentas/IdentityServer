using Identity.Server.Extended.Constants;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity.Server.Extended.Security;

/// <summary>
/// 
/// </summary>
public static class ApiScopes
{
    /// <summary>
    /// User Claims
    /// </summary>
    public static readonly ICollection<string> UserClaims = new[]
    {
        ExtendedClaimTypes.DeviceId,
        ExtendedClaimTypes.IPAddress,
        ExtendedClaimTypes.Scope,
        JwtClaimTypes.Email,
        JwtClaimTypes.EmailVerified,
        JwtClaimTypes.FamilyName,
        JwtClaimTypes.GivenName,
        JwtClaimTypes.Name,
        JwtClaimTypes.PhoneNumber,
        JwtClaimTypes.PhoneNumberVerified,
        JwtClaimTypes.Role,
        JwtClaimTypes.Subject
    };

    /// <summary>
    /// Test API
    /// </summary>
    public static ApiScope TestApi => new()
    {
        // - Resource Name in the Api's appsettings.json
        Name = "test-api",
        // - Friendly Name in the Api's appsettings.json
        DisplayName = "Test API",
        
        UserClaims = UserClaims,
        
        Description = "Access to the Test API",
    };
    
    /// <summary>
    /// Get Api Scopes
    /// </summary>
    /// <returns></returns>
    public static ApiScope[] GetApiScopes()
    {
        return new[]
        {
            TestApi, IdentityServerAdminClient, ClientsWrite, ClientsRead, UsersWrite, UsersRead, ScopesWrite, ScopesRead, ResourcesWrite, ResourcesRead, RolesWrite, RolesRead
        };
    }
    
    /// <summary>
    /// Identity Server Admin Client Scope Gives Access To All The Admin Management APIs
    /// </summary>
    public static ApiScope IdentityServerAdminClient => new()
    {
        Name = "identity-server-admin-client",
        DisplayName = "Identity Server Admin Client",
        Description = "Access to the Identity Server Admin Client",
        UserClaims = UserClaims
    };
    
    /// <summary>
    /// Clients Write Scope Is Needed For The Client Management API Only When A System Client Is Requesting To Write To The Clients Resource
    /// </summary>
    public static ApiScope ClientsWrite
    {
        get
        {
            return new ApiScope
            {
                Name = "clients.write",
                DisplayName = "Clients Write",
                Description = "Write access to the Clients resource",
                UserClaims = UserClaims
            };
        }
    } 
    
    /// <summary>
    /// Clients Read Scope Is Needed For The Client Management API Only When A System Client Is Requesting To Read From The Clients Resource
    /// </summary>
    public static ApiScope ClientsRead
    {
        get
        {
            return new ApiScope
            {
                Name = "clients.read",
                DisplayName = "Clients Read",
                Description = "Read access to the Clients resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Users Write Scope Is Needed For The User Management API Only When A System Client Is Requesting To Write To The Users Resource
    /// </summary>
    public static ApiScope UsersWrite
    {
        get
        {
            return new ApiScope
            {
                Name = "users.write",
                DisplayName = "Users Write",
                Description = "Write access to the Users resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Users Read Scope Is Needed For The User Management API Only When A System Client Is Requesting To Read From The Users Resource
    /// </summary>
    public static ApiScope UsersRead
    {
        get
        {
            return new ApiScope
            {
                Name = "users.read",
                DisplayName = "Users Read",
                Description = "Read access to the Users resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Scopes Write Scope Is Needed For The Scope Management API Only When A System Client Is Requesting To Write To The Scopes Resource
    /// </summary>
    public static ApiScope ScopesWrite
    {
        get
        {
            return new ApiScope
            {
                Name = "scopes.write",
                DisplayName = "Scopes Write",
                Description = "Write access to the Scopes resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Scopes Read Scope Is Needed For The Scope Management API Only When A System Client Is Requesting To Read From The Scopes Resource
    /// </summary>
    public static ApiScope ScopesRead
    {
        get
        {
            return new ApiScope
            {
                Name = "scopes.read",
                DisplayName = "Scopes Read",
                Description = "Read access to the Scopes resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Resources Write Scope Is Needed For The Resource Management API Only When A System Client Is Requesting To Write To The Resources Resource
    /// </summary>
    public static ApiScope ResourcesWrite
    {
        get
        {
            return new ApiScope
            {
                Name = "resources.write",
                DisplayName = "Resources Write",
                Description = "Write access to the Resources resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Resources Read Scope Is Needed For The Resource Management API Only When A System Client Is Requesting To Read From The Resources Resource
    /// </summary>
    public static ApiScope ResourcesRead
    {
        get
        {
            return new ApiScope
            {
                Name = "resources.read",
                DisplayName = "Resources Read",
                Description = "Read access to the Resources resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Roles Write Scope Is Needed For The Role Management API Only When A System Client Is Requesting To Write To The Roles Resource
    /// </summary>
    public static ApiScope RolesWrite
    {
        get
        {
            return new ApiScope
            {
                Name = "roles.write",
                DisplayName = "Roles Write",
                Description = "Write access to the Roles resource",
                UserClaims = UserClaims
            };
        }
    }
    
    /// <summary>
    /// Roles Read Scope Is Needed For The Role Management API Only When A System Client Is Requesting To Read From The Roles Resource
    /// </summary>
    public static ApiScope RolesRead
    {
        get
        {
            return new ApiScope
            {
                Name = "roles.read",
                DisplayName = "Roles Read",
                Description = "Read access to the Roles resource",
                UserClaims = UserClaims
            };
        }
    }
}