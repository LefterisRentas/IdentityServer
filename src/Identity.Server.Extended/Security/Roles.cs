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
    public const string UsersRead = nameof(UsersRead);
    /// <summary>
    /// UsersWrite role for the user management api
    /// </summary>
    public const string UsersWrite = nameof(UsersWrite);
    /// <summary>
    /// ClientsRead role for the client management api
    /// </summary>
    public const string ClientsRead = nameof(ClientsRead);
    /// <summary>
    /// ClientsWrite role for the client management api
    /// </summary>
    public const string ClientsWrite = nameof(ClientsWrite);
    /// <summary>
    /// ScopesRead role for the scope management api
    /// </summary>
    public const string ScopesRead = nameof(ScopesRead);
    /// <summary>
    /// ScopesWrite role for the scope management api
    /// </summary>
    public const string ScopesWrite = nameof(ScopesWrite);
    /// <summary>
    /// ResourcesRead role for the resource management api
    /// </summary>
    public const string ResourcesRead = nameof(ResourcesRead);
    /// <summary>
    /// ResourcesWrite role for the resource management api
    /// </summary>
    public const string ResourcesWrite = nameof(ResourcesWrite);
    
    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IdentityRole> GetRoles()
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole(Admin),
            new IdentityRole(UsersRead),
            new IdentityRole(UsersWrite),
            new IdentityRole(ClientsRead),
            new IdentityRole(ClientsWrite),
            new IdentityRole(ScopesRead),
            new IdentityRole(ScopesWrite),
            new IdentityRole(ResourcesRead),
            new IdentityRole(ResourcesWrite)
        };
        return roles;
    }
}