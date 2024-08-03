using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Identity.Server.MVC.Security;

public static class Roles
{
    public const string Admin = nameof(Admin);
    public const string UsersRead = nameof(UsersRead);
    public const string UsersWrite = nameof(UsersWrite);
    public const string ClientsRead = nameof(ClientsRead);
    public const string ClientsWrite = nameof(ClientsWrite);
    public const string ScopesRead = nameof(ScopesRead);
    public const string ScopesWrite = nameof(ScopesWrite);
    public const string ResourcesRead = nameof(ResourcesRead);
    public const string ResourcesWrite = nameof(ResourcesWrite);
    
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