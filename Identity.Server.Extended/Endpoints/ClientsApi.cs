using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Endpoints.Handlers;
using Identity.Server.Extended.Security;
using Identity.Server.MVC.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Server.Extended.Endpoints;

/// <summary>
/// Clients API endpoints.
/// </summary>
public static class ClientsApi
{

    /// <summary>
    /// Maps the clients API.
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public static void MapClients(this IEndpointRouteBuilder routes)
    {
        var options = routes.ServiceProvider.GetService<IConfiguration>();
        var readClientsGroup = routes.MapGroup(API_PREFIXES.CLIENTS_API_PREFIX);
        readClientsGroup.WithTags("Clients");
        // Add security requirements, all incoming requests to this API *must*
        // be authenticated with a valid user.
        //TODO: Extract this authorization policy to a shared location.
        readClientsGroup.RequireAuthorization(pb => pb
            .RequireAuthenticatedUser()
            .RequireAssertion(x => x.User.IsInRole(Roles.ClientsRead) 
                                   || x.User.IsInRole(Roles.Admin) 
                                   || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.ClientsRead.Name)
                                   || x.User.HasClaim(ExtendedClaimTypes.Scope, ApiScopes.IdentityServerAdminClient.Name)
        ));
        readClientsGroup.WithOpenApi();

        readClientsGroup.MapGet("", ClientHandler.GetClients)
            .WithName(nameof(ClientHandler.GetClients));
        
        readClientsGroup.MapGet("/{clientId:minlength(1)}", ClientHandler.GetClientById)
            .WithName(nameof(ClientHandler.GetClientById));
    }
}