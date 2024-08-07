using System.Collections.Generic;
using Identity.Server.Extended.Constants;
using IdentityModel;
using IdentityServer4.Models;

namespace Identity.Server.MVC.Security;

public static class ApiScopes
{
    public static readonly ICollection<string> UserClaims = new[]
    {
        ExtendedClaimTypes.DeviceId,
        ExtendedClaimTypes.IPAddress,
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

    public static ApiScope TestApi => new()
    {
        // - Resource Name in the Api's appsettings.json
        Name = "test-api",
        // - Friendly Name in the Api's appsettings.json
        DisplayName = "Test API",
        
        UserClaims = UserClaims,
        
        Description = "Access to the Test API",
    };
}