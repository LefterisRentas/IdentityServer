using Microsoft.AspNetCore.Identity;

namespace Identity.Server.Extended.Security;

/// <summary>
/// Roles
/// </summary>
public static class Roles
{
    /// <summary>
    /// Admin role
    /// </summary>
    public const string Admin = nameof(Admin);
    /// <summary>
    /// UsersRead role for the user management api
    /// </summary>
    public const string UsersApiRead = nameof(UsersApiRead);
    /// <summary>
    /// UsersWrite role for the user management api
    /// </summary>
    public const string UsersApiWrite = nameof(UsersApiWrite);
    /// <summary>
    /// ClientsRead role for the client management api
    /// </summary>
    public const string ClientsApiRead = nameof(ClientsApiRead);
    /// <summary>
    /// ClientsWrite role for the client management api
    /// </summary>
    public const string ClientsApiWrite = nameof(ClientsApiWrite);
    /// <summary>
    /// ScopesRead role for the scope management api
    /// </summary>
    public const string ScopesApiRead = nameof(ScopesApiRead);
    /// <summary>
    /// ScopesWrite role for the scope management api
    /// </summary>
    public const string ScopesApiWrite = nameof(ScopesApiWrite);
    /// <summary>
    /// ResourcesRead role for the resource management api
    /// </summary>
    public const string ResourcesApiRead = nameof(ResourcesApiRead);
    /// <summary>
    /// ResourcesWrite role for the resource management api
    /// </summary>
    public const string ResourcesApiWrite = nameof(ResourcesApiWrite);
    /// <summary>
    /// IdentityResourcesRead role for the identity resource management api
    /// </summary>
    public const string IdentityResourcesApiRead = nameof(IdentityResourcesApiRead);
    /// <summary>
    /// IdentityResourcesWrite role for the identity resource management api
    /// </summary>
    public const string IdentityResourcesApiWrite = nameof(IdentityResourcesApiWrite);
    /// <summary>
    /// RolesRead role for the role management api
    /// </summary>
    public const string RolesApiRead = nameof(RolesApiRead);
    /// <summary>
    /// RolesWrite role for the role management api
    /// </summary>
    public const string RolesApiWrite = nameof(RolesApiWrite);
    
    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IdentityRole> GetRoles()
    {
        var roles = new List<IdentityRole>
        {
            new(Admin),
            new(UsersApiRead),
            new(UsersApiWrite),
            new(ClientsApiRead),
            new(ClientsApiWrite),
            new(ScopesApiRead),
            new(ScopesApiWrite),
            new(ResourcesApiRead),
            new(ResourcesApiWrite),
            new(IdentityResourcesApiRead),
            new(IdentityResourcesApiWrite),
            new(RolesApiRead),
            new(RolesApiWrite)
        };
        return roles;
    }
}