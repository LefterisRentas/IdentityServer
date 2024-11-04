using Identity.Server.Extended.Constants;
using Identity.Server.Extended.Endpoints.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Identity.Server.Extended.Endpoints;

/// <summary>
/// Clients Management API endpoints.
/// </summary>
public static class ClientsApi
{

    /// <summary>
    /// Maps the clients management API.
    /// </summary>
    /// <param name="routes"></param>
    /// <returns></returns>
    public static void MapClientsManagement(this IEndpointRouteBuilder routes)
    {
        //var options = routes.ServiceProvider.GetService<IConfiguration>();
        var readClientsGroup = routes.MapGroup(API_PREFIXES.CLIENTS_MANAGEMENT_API_PREFIX);
        readClientsGroup.WithTags("Clients");
        readClientsGroup.RequireAuthorization(AuthorizationPolicyConstants.CLIENT_MANAGEMENT_READ);
        readClientsGroup.WithOpenApi();

        readClientsGroup.MapGet("", ClientsHandler.GetClients)
            .WithName(nameof(ClientsHandler.GetClients));
        
        readClientsGroup.MapGet("/{clientId:minlength(1)}", ClientsHandler.GetClientById)
            .WithName(nameof(ClientsHandler.GetClientById));
        var writeClientsGroup = routes.MapGroup(API_PREFIXES.CLIENTS_MANAGEMENT_API_PREFIX);
        writeClientsGroup.WithTags("Clients");
        writeClientsGroup.RequireAuthorization(AuthorizationPolicyConstants.CLIENT_MANAGEMENT_WRITE);
        writeClientsGroup.WithOpenApi();
    }
}