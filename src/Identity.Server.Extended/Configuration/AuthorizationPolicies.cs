using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Configuration;

/// <summary>
/// Configures the authorization policies.
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// Adds the authorization configuration.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IServiceCollection AddExtendedIdentityServerAuthorizationConfig(this WebApplicationBuilder builder)
    {
        return builder.Services.AddAuthorization(authOptions =>
        {
            authOptions.AddPolicy(AuthorizationPolicyConstants.API_RESOURCE_MANAGEMENT_READ, pb => pb.RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ResourcesApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ResourcesRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.CLIENT_MANAGEMENT_READ, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ClientsApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ClientsRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.IDENTITY_RESOURCE_MANAGEMENT_READ, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.IdentityResourcesApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ResourcesRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.ROLE_MANAGEMENT_READ, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.RolesApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.RolesRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.SCOPE_MANAGEMENT_READ, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ScopesApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ScopesRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.USER_MANAGEMENT_READ, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.UsersApiRead)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.UsersRead.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.API_RESOURCE_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ResourcesApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ResourcesWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ClientsApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ClientsWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.IDENTITY_RESOURCE_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.IdentityResourcesApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ResourcesWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.ROLE_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.RolesApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.RolesWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.SCOPE_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.ScopesApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ScopesWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
            authOptions.AddPolicy(AuthorizationPolicyConstants.USER_MANAGEMENT_WRITE, pb => pb
                .RequireAuthenticatedUser()
                .RequireAssertion(x => x.User.IsInRole(Roles.UsersApiWrite)
                                       || x.User.IsInRole(Roles.Admin)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.UsersWrite.Name)
                                       || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
                ));
        });
    }
}